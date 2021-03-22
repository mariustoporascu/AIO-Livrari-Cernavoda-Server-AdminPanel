import axios from "axios";
import React, { createContext, useEffect, useState } from "react";

export const OrderContext = createContext();

const OrderContextProvider = (props) => {
  const [orders, setOrders] = useState([]);
  const [orderInfo, setOrderInfo] = useState([]);
  const [productsInOrder, setProductsInOrder] = useState([]);
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [modal, setModal] = useState(false);

  useEffect(() => {
    const populateState = async () => {
      await axios
        .get("AdminPanel/getallorders")
        .then((response) => setOrders(response.data))
        .catch((error) => console.log(error));
    };
    populateState();
    if (isLoading) {
      setIsLoading(false);
    }
  }, [isLoading]);

  const getOrderInfo = async (orderId) => {
    await axios
      .get("AdminPanel/getorderinfo/" + orderId)
      .then((response) => setOrderInfo(response.data))
      .catch((error) => console.log(error));
    await axios
      .get("AdminPanel/getproductsinorder/" + orderId)
      .then((response) => setProductsInOrder(response.data))
      .catch((error) => console.log(error));
    await axios
      .get("AdminPanel/getproductsfororder/" + orderId)
      .then((response) => setProducts(response.data))
      .catch((error) => console.log(error));
  };

  const toggleModal = () => {
    setModal(!modal);
  };

  const clearData = () => {
    setOrderInfo([]);
    setProducts([]);
    setProductsInOrder([]);
  };

  return (
    <OrderContext.Provider
      value={{
        orders,
        orderInfo,
        productsInOrder,
        products,
        modal,
        getOrderInfo,
        toggleModal,
        clearData,
      }}
    >
      {props.children}
    </OrderContext.Provider>
  );
};

export default OrderContextProvider;
