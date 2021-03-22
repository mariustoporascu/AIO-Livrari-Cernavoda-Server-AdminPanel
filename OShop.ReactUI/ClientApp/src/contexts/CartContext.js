import React, { createContext, useEffect, useState } from "react";
import authService from "../components/api-authorization/AuthorizeService";
import axios from "axios";

export const CartContext = createContext();

const CartContextProvider = (props) => {
  const [isAuthenticatedUser, setIsAuthenticatedUser] = useState(false);
  const [userInfo, setUserInfo] = useState(null);
  const [cart, setCart] = useState([]);
  const [productsInCart, setProductsInCart] = useState([]);
  const [cartItems, setCartItems] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [orderInfoId, setOrderInfoId] = useState(0);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [address, setAddress] = useState("");
  const [phoneNo, setPhoneNo] = useState("");
  const [orderRefId, setOrderRefId] = useState(0);
  const [disabledBtn, setDisabledBtn] = useState(true);

  useEffect(() => {
    const populateState = async () => {
      const [isAuthenticated, user] = await Promise.all([
        authService.isAuthenticated(),
        authService.getUser(),
      ]);

      setIsAuthenticatedUser(isAuthenticated);
      if (!!user) {
        setUserInfo(user);
      } else {
        setUserInfo("undefined");
      }
    };
    populateState();
  }, []);

  useEffect(() => {
    const getCartData = async () => {
      if (userInfo === "undefined") {
        await axios
          .get("ShoppingCart/getcart/" + userInfo)
          .then((response) => setCart(response.data))
          .catch((error) => console.log(error));
      } else {
        await axios
          .get("ShoppingCart/getcart/" + userInfo.sub)
          .then((response) => setCart(response.data))
          .catch((error) => console.log(error));
      }
    };
    if (!!userInfo) {
      getCartData();
    }
    if (isLoading) {
      setIsLoading(!isLoading);
    }
  }, [userInfo, isLoading]);

  useEffect(() => {
    const getCartItems = async () => {
      await axios
        .get("ShoppingCart/getproductincart/" + cart.cartId)
        .then((response) => setProductsInCart(response.data))
        .catch((error) => console.log(error));
      await axios
        .get("ShoppingCart/getcartitems/" + cart.cartId)
        .then((response) => setCartItems(response.data))
        .catch((error) => console.log(error));
    };

    if (cart.length !== 0) {
      getCartItems();
    }
  }, [cart]);

  useEffect(() => {
    const populateOrderInfo = async () => {
      let orderInfo;
      if (userInfo === "undefined") {
        await axios
          .get("ShoppingCart/orderinfo/" + userInfo)
          .then((response) => (orderInfo = response.data))
          .catch((error) => console.log(error));
      } else {
        await axios
          .get("ShoppingCart/orderinfo/" + userInfo.sub)
          .then((response) => (orderInfo = response.data))
          .catch((error) => console.log(error));
      }
      if (orderInfo.length !== 0) {
        if (!!orderInfo.orderInfoId) {
          setOrderInfoId(orderInfo.orderInfoId);
          setFirstName(orderInfo.firstName);
          setLastName(orderInfo.lastName);
          setAddress(orderInfo.address);
          setPhoneNo(orderInfo.phoneNo);
          setOrderRefId(orderInfo.orderRefId);
          setDisabledBtn(false);
        } else {
          setOrderRefId(orderInfo.orderRefId);
        }
      }
    };
    if (!!userInfo) {
      populateOrderInfo();
    }
  }, [userInfo]);

  const setValues = (event) => {
    let name = event.target.name;
    let value = event.target.value;

    switch (name) {
      case "firstName":
        setFirstName(value);
        break;
      case "lastName":
        setLastName(value);
        break;
      case "address":
        setAddress(value);
        break;
      case "phoneNo":
        setPhoneNo(value);
        break;
      default:
        break;
    }
    setCustomValidity(event);
  };

  const setCustomValidity = (event) => {
    event.preventDefault();
    let name = event.target.name;
    let value = event.target.value;
    if (value === "") {
      document.getElementById("span " + name).textContent =
        "This cannot be empty";
    } else {
      document.getElementById("span " + name).textContent = "";
    }
  };

  const toggleReload = () => {
    setIsLoading(!isLoading);
  };

  return (
    <CartContext.Provider
      value={{
        isAuthenticatedUser,
        userInfo,
        cart,
        cartItems,
        productsInCart,
        orderInfoId,
        firstName,
        lastName,
        address,
        phoneNo,
        orderRefId,
        disabledBtn,
        setValues,
        setCustomValidity,
        setDisabledBtn,
        toggleReload,
      }}
    >
      {props.children}
    </CartContext.Provider>
  );
};

export default CartContextProvider;
