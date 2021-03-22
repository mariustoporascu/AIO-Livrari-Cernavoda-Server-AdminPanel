import axios from "axios";
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { CartContext } from "../../contexts/CartContext";
import { DataContext } from "../../contexts/DataContext";

const Checkout = () => {
  const { toast } = useContext(DataContext);
  const {
    userInfo,
    cart,
    cartItems,
    productsInCart,
    firstName,
    lastName,
    address,
    phoneNo,
    orderRefId,
    disabledBtn,
  } = useContext(CartContext);

  const placeOrder = async (event) => {
    event.preventDefault();
    const form = new FormData();
    form.append("orderId", orderRefId);
    form.append("status", "Ordered");
    form.append("customerId", cart.customerId);
    form.append("totalOrdered", cart.totalInCart);
    await axios
      .put("ShoppingCart/placeorder", form)
      .then(() =>
        toast.success("Congratulations, your order was successfull!", {
          autoClose: 2000,
        })
      )
      .catch((error) => console.log(error));
    if (userInfo !== "undefined") {
      setTimeout(function () {
        const redirectUrl = `${window.location.origin}/Identity/Account/Manage/Orders`;

        window.location.replace(redirectUrl);
      }, 2000);
    } else {
      setTimeout(function () {
        const redirectUrl = `${window.location.origin}/`;

        window.location.replace(redirectUrl);
      }, 2000);
    }
  };

  return (
    <div>
      <h1>Checkout</h1>
      <table className="table table-striped">
        <thead>
          <tr>
            <th>Product</th>
            <th>Quantity</th>
            <th>Price</th>
            <th>Photo</th>
          </tr>
        </thead>
        <tbody>
          {productsInCart.map((product) => (
            <tr key={product.productId}>
              <td>{product.name}</td>
              <td>
                {
                  cartItems.filter(
                    (item) => item.productRefId === product.productId
                  )[0].quantity
                }
              </td>
              <td>
                {(
                  product.price *
                  cartItems.filter(
                    (item) => item.productRefId === product.productId
                  )[0].quantity
                ).toFixed(2)}
              </td>
              <td>
                <img
                  id="productphoto"
                  style={{
                    width: 50 + "px",
                    height: 50 + "px",
                    objectFit: "cover",
                  }}
                  src={`WebImage/GetImage/${product.photo}`}
                  alt="product"
                />
              </td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <th></th>
            <th></th>
            <th></th>

            <th>Total: {cart.totalInCart.toFixed(2)}</th>
          </tr>
        </tfoot>
      </table>
      <table className="table table-striped">
        <thead></thead>
        <tbody>
          <tr>
            <th>First Name</th>
            <td>{firstName}</td>
          </tr>
          <tr>
            <th>Last Name</th>
            <td>{lastName}</td>
          </tr>
          <tr>
            <th>Address</th>
            <td>{address}</td>
          </tr>
          <tr>
            <th>Phone No</th>
            <td>{phoneNo}</td>
          </tr>
        </tbody>
      </table>

      {disabledBtn ? (
        <span
          className="d-inline-block"
          tabIndex="0"
          data-toggle="tooltip"
          title="You must add order info to proceed"
          style={{ float: "right" }}
        >
          <button
            disabled="true"
            className="btn btn-default"
            style={{ margin: 1 + "em", float: "right" }}
            onClick={placeOrder}
          >
            Place Order
          </button>
        </span>
      ) : (
        <button
          className="btn btn-success"
          style={{ margin: 1 + "em", float: "right" }}
          onClick={placeOrder}
        >
          Place Order
        </button>
      )}
      <Link
        tag={Link}
        className="btn btn-warning"
        style={{ margin: 1 + "em", float: "right" }}
        to="/shoppingcart"
      >
        Go Back
      </Link>
    </div>
  );
};
export default Checkout;
