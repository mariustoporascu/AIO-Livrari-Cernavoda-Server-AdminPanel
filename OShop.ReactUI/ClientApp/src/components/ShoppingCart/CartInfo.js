import axios from "axios";
import React, { useContext, useEffect, useState } from "react";

import OrderInfo from "./OrderInfo";

import { CartContext } from "../../contexts/CartContext";
import { DataContext } from "../../contexts/DataContext";

import { Link } from "react-router-dom";

const CartInfo = () => {
  const { cart, cartItems, productsInCart, toggleReload } = useContext(
    CartContext
  );
  const { toast } = useContext(DataContext);

  const selectQuantity = (product) => {
    let quantitySelection = [];
    let max = product.stock;
    var i;
    if (max < 20) {
      for (i = 1; i < max; i++) {
        quantitySelection.push(i);
      }
    } else {
      for (i = 1; i < 21; i++) {
        quantitySelection.push(i);
      }
    }
    return quantitySelection;
  };

  const updateQuantity = async (event) => {
    var productRefId = parseInt(event.target.name);
    var newQuantity = parseInt(event.target.value);
    var price = parseFloat(event.target.id);
    var cartIndex = cartItems.findIndex(
      (obj) => obj.productRefId === productRefId
    );
    var prevCartItem = cartItems[cartIndex];

    const form = new FormData();
    form.append("cartRefId", cart.cartId);
    form.append("productRefId", productRefId);
    form.append("quantity", newQuantity);
    form.append("price", price);
    form.append("prevQuantity", prevCartItem.quantity);
    await axios
      .put("ShoppingCart/updatecartitem/", form)
      .then(() => toast.success("Updated", { autoClose: 2000 }))
      .catch(() => toast.error("Error", { autoClose: 2000 }));

    toggleReload();
  };

  const removeCartItem = async (event) => {
    var cartIndex = cartItems.findIndex(
      (obj) => obj.productRefId === parseInt(event.target.value)
    );
    var cartItem = cartItems[cartIndex];
    var productPrice = productsInCart[cartIndex].price;

    await axios
      .delete("ShoppingCart/removecartitem", {
        params: {
          CartRefId: cartItem.cartRefId,
          ProductRefId: cartItem.productRefId,
          Quantity: cartItem.quantity,
          Price: productPrice,
        },
      })
      .then(() => toast.success("Removed", { autoClose: 2000 }))
      .catch(() => toast.error("Error", { autoClose: 2000 }));

    toggleReload();
  };

  return (
    <div>
      <h1>Shopping Cart</h1>
      {cart.length !== 0 &&
      cartItems.length !== 0 &&
      productsInCart.length !== 0 ? (
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Product</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Photo</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {productsInCart.map((product) => (
              <tr key={product.productId}>
                <td>{product.name}</td>
                <td>
                  <select
                    id={product.price}
                    name={product.productId}
                    value={
                      cartItems.filter(
                        (item) => item.productRefId === product.productId
                      )[0].quantity
                    }
                    className="form-select"
                    onChange={updateQuantity}
                  >
                    {selectQuantity(product).map((select) => (
                      <option key={select} value={select}>
                        {select}
                      </option>
                    ))}
                  </select>
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

                <td>
                  <button
                    type="submit"
                    value={product.productId}
                    className="btn btn-outline-danger btn-sm"
                    onClick={removeCartItem}
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <th></th>
              <th></th>
              <th></th>
              <th></th>
              <th>Total: {cart.totalInCart.toFixed(2)}</th>
            </tr>
          </tfoot>
        </table>
      ) : (
        <div className="text-center">
          <h4 style={{ marginTop: 20 + "%" }}>Your cart is empty</h4>

          <Link
            tag={Link}
            className="btn btn-info"
            style={{ margin: 1 + "em" }}
            to="/"
          >
            Continue Shopping
          </Link>
        </div>
      )}
      {cartItems.length !== 0 ? <OrderInfo /> : null}
    </div>
  );
};

export default CartInfo;
