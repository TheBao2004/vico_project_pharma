using Microsoft.EntityFrameworkCore;
using Pharma.Data;
using Pharma.Models;

namespace Pharma.Services
{
	public partial class ShoppingCart
	{


		//MusicStoreEntities _context = new MusicStoreEntities();
		//public AppDbContext _context = new AppDbContext();
		public IHttpContextAccessor _contextAccessor;
		public readonly AppDbContext _context;
		string ShoppingCartId { get; set; }
		public const string CartSessionKey = "CartId";

		public ShoppingCart(AppDbContext context, IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
			_context = context;
		}

		public static ShoppingCart GetCart(AppDbContext context, IHttpContextAccessor contextAccessor)
		{
			var cart = new ShoppingCart(context, contextAccessor);
			cart.ShoppingCartId = cart.GetCartId(context);
			return cart;
		}
		// Helper method to simplify shopping cart calls
		//public static ShoppingCart GetCart(Controller controller)
		//{
		//	return GetCart(controller.HttpContext);
		//}
		public object AddToCart(ProductModel model)
		{
			// Get the matching cart and Product instances

			var cartItem = _context.CartModel.SingleOrDefault(
				c => c.CartId == ShoppingCartId
				&& c.ProductId == model.Id);

			if (cartItem == null)
			{
				// Create a new cart item if no cart item exists
				cartItem = new CartModel
				{
					ProductId = model.Id,
					CartId = ShoppingCartId,
					Count = 1,
					DateCreated = DateTime.Now,
				};
				_context.CartModel.Add(cartItem);
			}
			else
			{
				// If the item does exist in the cart, 
				// then add one to the quantity
				cartItem.Count++;
			}
			// Save changes
			_context.SaveChanges();

			return new
			{
				Msg = "Them San Pham Thanh Cong",
				Total = model.Price * cartItem.Count,
				Sum_Total = GetTotal()
			};
		}

		public object AddToCartDetail(ProductModel model, int quantity)
		{
			// Get the matching cart and Product instances

			var cartItem = _context.CartModel.SingleOrDefault(
				c => c.CartId == ShoppingCartId
				&& c.ProductId == model.Id);

			if (cartItem == null)
			{
				// Create a new cart item if no cart item exists
				cartItem = new CartModel
				{
					ProductId = model.Id,
					CartId = ShoppingCartId,
					Count = quantity,
					DateCreated = DateTime.Now,
				};
				_context.CartModel.Add(cartItem);
			}
			else
			{
				// If the item does exist in the cart, 
				// then add one to the quantity
				cartItem.Count += quantity;
			}
			// Save changes
			_context.SaveChanges();

			return new
			{
				Msg = "Them San Pham Thanh Cong",
				Total = model.Price * cartItem.Count,
				Sum_Total = GetTotal()
			};
		}

		public object UpdateCartItem(ProductModel model, int quantity)
		{
			// Get the matching cart and Product instances

			var cartItem = _context.CartModel.SingleOrDefault(
				c => c.CartId == ShoppingCartId
				&& c.ProductId == model.Id);

			if(cartItem == null){
				return "";
			}

			var empty = 0;

			if(quantity == 0){
				empty = 1;
				_context.Remove(cartItem);
			}
			cartItem.Count = quantity;

			// Save changes
			_context.SaveChanges();

			int Price = model.Price;

			if(model.Discount > 0) Price = model.Discount ?? 0;

			return new
			{
				Empty = empty,
				itemTotal = Price * cartItem.Count,
				Total = GetTotal()
			};
		}

		public object MinusToCart(ProductModel model)
		{
			// Get the matching cart and Product instances

			var cartItem = _context.CartModel.SingleOrDefault(
				c => c.CartId == ShoppingCartId
				&& c.ProductId == model.Id);

			if (cartItem != null)
			{
				// Create a new cart item if no cart item exists

				cartItem.Count--;

				if (cartItem.Count <= 0)
				{
					_context.CartModel.Remove(cartItem);
				}

			}

			// Save changes
			_context.SaveChanges();

			return new
			{
				Msg = "Giam San Pham Thanh Cong",
				Total = model.Price * cartItem.Count,
				Sum_Total = GetTotal()
			};

		}

		public object RemoveFromCart(int id)
		{
			// Get the cart
			var cartItem = _context.CartModel.Single(
				cart => cart.CartId == ShoppingCartId
				&& cart.Id == id);

			int itemCount = 0;

			if (cartItem != null)
			{
				_context.CartModel.Remove(cartItem);
				// Save changes
				_context.SaveChanges();
			}
			return new
			{
				Msg = "Xoa Thanh Cong",
				Sum_Total = GetTotal()
			};
		}
		public object EmptyCart()
		{
			var cartItems = _context.CartModel.Where(
				cart => cart.CartId == ShoppingCartId);

			foreach (var cartItem in cartItems)
			{
				_context.CartModel.Remove(cartItem);
			}
			// Save changes
			_context.SaveChanges();

			return new
			{
				Msg = "Gio Hang Lam Trong Thanh Cong"
			};

		}
		public List<CartModel> GetCartItems()
		{
			return _context.CartModel.Include(c => c.Product).Where(
				cart => cart.CartId == ShoppingCartId).ToList();
		}
		public int GetCount()
		{
			// Get the count of each item in the cart and sum them up
			int? count = (from cartItems in _context.CartModel
						  where cartItems.CartId == ShoppingCartId
						  select (int?)cartItems.Count).Sum();
			// Return 0 if all entries are null
			return count ?? 0;
		}
		public decimal GetTotal()
		{
			// Multiply Product price by count of that Product to get 
			// the current price for each of those Products in the cart
			// sum all Product price totals to get the cart total
			decimal? total = (from cartItems in _context.CartModel
							  where cartItems.CartId == ShoppingCartId
							  select (int?)cartItems.Count *
							  (cartItems.Product.Discount > 0 ? cartItems.Product.Discount : cartItems.Product.Price)).Sum();

			return total ?? decimal.Zero;
		}
		public int CreateOrder(OrderModel order)
		{
			decimal orderTotal = 0;

			var cartItems = GetCartItems();
			// Iterate over the items in the cart, 
			// adding the order details for each
			foreach (var item in cartItems)
			{
				var orderDetail = new DetailOrderModel
				{
					ProductId = item.ProductId,
					OrderId = order.OrderId,
					Quantity = item.Count
				};
				// Set the order total of the shopping cart
				orderTotal += (item.Count * item.Product.Price);

				_context.DetailOrderModel.Add(orderDetail);

			}
			// Set the order's total to the orderTotal count
			order.Total = orderTotal;

			// Save the order
			_context.SaveChanges();
			// Empty the shopping cart
			EmptyCart();
			// Return the OrderId as the confirmation number
			return order.OrderId;
		}
		// We're using HttpContextBase to allow access to cookies.
		public string GetCartId(AppDbContext context)
		{

			if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				var unique = _contextAccessor.HttpContext.User.Identity.Name;
				if (!string.IsNullOrWhiteSpace(unique))
				{
					_contextAccessor.HttpContext.Session.SetString("CartSessionKey", unique);
				}
				else
				{
					// Generate a new random GUID using System.Guid class
					Guid tempCartId = Guid.NewGuid();
					// Send tempCartId back to client as a cookie
					_contextAccessor.HttpContext.Session.SetString("CartSessionKey", tempCartId.ToString());
				}
			}
			else
			{
				if (_contextAccessor.HttpContext.Session.GetString("CartSessionKey") == null)
				{
					// Generate a new random GUID using System.Guid class
					Guid tempCartId = Guid.NewGuid();
					// Send tempCartId back to client as a cookie
					_contextAccessor.HttpContext.Session.SetString("CartSessionKey", tempCartId.ToString());
				}
			}


			return _contextAccessor.HttpContext.Session.GetString("CartSessionKey");
		}

		// public void AddToCartLogin(UserModel user, string old)
		// {
		// 	var allCartItems = _context.CartModel.Include(c => c.Product).Where(c => c.CartId == old).ToList();
		// 	var allCartItemsLogin = _context.CartModel.Include(c => c.Product).Where(c => c.CartId == user.Fullname).ToList();
		// 	var login = user.Fullname;

		// 	if (allCartItemsLogin.Count() >= allCartItems.Count())
		// 	{
		// 		foreach (var itemLogin in allCartItemsLogin)
		// 		{

		// 			//var item = allCartItems.FirstOrDefault(i => i.ProductId == itemLogin.ProductId);
		// 			var item = (from c in allCartItems
		// 						join p in _context.ProductModel.ToList()
		// 						on c.ProductId equals p.Id
		// 						where c.ProductId == itemLogin.ProductId
		// 						select new
		// 						{
		// 							C = c,
		// 							P = p
		// 						}).FirstOrDefault();

		// 			if (item != null)
		// 			{
		// 				itemLogin.Count += item.C.Count;
		// 				_context.CartModel.Update(itemLogin);
		// 			}
		// 			else
		// 			{
		// 				var cartItem = new CartModel()
		// 				{
		// 					CartId = login,
		// 					ProductId = item.C.ProductId,
		// 					Count = item.C.Count,
		// 					DateCreated = item.C.DateCreated,
		// 				};
		// 				_context.CartModel.Add(cartItem);
		// 			}

		// 			_context.CartModel.Remove(allCartItems.FirstOrDefault(i => i.ProductId == itemLogin.ProductId));

		// 		}
		// 	}
		// 	else
		// 	{
		// 		foreach (var item in allCartItems)
		// 		{

		// 			var itemLogin = allCartItemsLogin.FirstOrDefault(c => c.ProductId == item.ProductId);

		// 			if (itemLogin != null)
		// 			{
		// 				itemLogin.Count += item.Count;
		// 				// itemLogin.Total = itemLogin.Count * itemLogin.Product.Price;
		// 				_context.CartModel.Update(itemLogin);
		// 			}
		// 			else
		// 			{
		// 				var cartItem = new CartModel()
		// 				{
		// 					CartId = login,
		// 					ProductId = item.ProductId,
		// 					Count = item.Count,
		// 					DateCreated = item.DateCreated,
		// 				};
		// 				_context.CartModel.Add(cartItem);
		// 			}

		// 			_context.CartModel.Remove(item);

		// 		}
		// 	}









		// 	//foreach (var item in allCartItems)
		// 	//    {
		// 	//        foreach (var itemLogin in allCartItemsLogin)
		// 	//        {

		// 	//            if (item.ProductId == itemLogin.ProductId)
		// 	//            {
		// 	//                var cartUpdate = _context.cartAjax.Where(c => c.CartId == itemLogin.CartId).FirstOrDefault();
		// 	//                cartUpdate.CartId = itemLogin.CartId;
		// 	//                cartUpdate.Count = item.Count + itemLogin.Count;
		// 	//                cartUpdate.Total = cartUpdate.Count * itemLogin.Product.Price;
		// 	//                _context.cartAjax.Update(cartUpdate);
		// 	//            }
		// 	//            //else
		// 	//            //{
		// 	//            //    var cartItem = new Eshopper.Models.ShoppingCartAjax.Cart()
		// 	//            //    {
		// 	//            //        CartId = itemLogin.CartId,
		// 	//            //        ProductId = item.ProductId,
		// 	//            //        Count = item.Count,
		// 	//            //        DateCreated = item.DateCreated,
		// 	//            //        Total = item.Product.Price * item.Count
		// 	//            //    };	
		// 	//            //    _context.cartAjax.Add(cartItem);
		// 	//            //}

		// 	//            _context.cartAjax.Remove(item);

		// 	//        }
		// 	//    }



		// 	//allCartItems = _context.cartAjax.Include(c => c.Product).Where(c => c.CartId == old).ToList();

		// 	//if (allCartItems.Count() >= 1) {
		// 	//             foreach (var item in allCartItems)
		// 	//             {

		// 	//		var cartItem = new Eshopper.Models.ShoppingCartAjax.Cart()
		// 	//		{
		// 	//			CartId = login,
		// 	//			ProductId = item.ProductId,
		// 	//			Count = item.Count,
		// 	//			DateCreated = item.DateCreated,
		// 	//			Total = item.Product.Price * item.Count
		// 	//		};
		// 	//		_context.cartAjax.Add(cartItem);

		// 	//		_context.cartAjax.Remove(item);

		// 	//             }
		// 	//         }


		// 	_context.SaveChangesAsync();

		// }




		// When a user has logged in, migrate their shopping cart to
		// be associated with their username
		public void MigrateCart(string userName)
		{
			var shoppingCart = _context.CartModel.Where(
				c => c.CartId == ShoppingCartId);

			foreach (var item in shoppingCart)
			{
				item.CartId = userName;
			}
			_context.SaveChanges();
		}


	}
}
