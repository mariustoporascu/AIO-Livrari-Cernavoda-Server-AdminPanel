import React from "react";
import OrderContextProvider from "../../../contexts/OrderContext";
import OrderInfo from "./OrderInfo";
import OrdersTable from "./OrdersTable";

const ManageOrders = () => {
  return (
    <div>
      <h1>Shop Orders</h1>
      <OrderContextProvider>
        <OrdersTable></OrdersTable>
        <OrderInfo></OrderInfo>
        {/* <Pagination></Pagination>
    <h3 style={{ marginTop: 1 + "em" }}>Add/Update Category</h3>
    <CategoriesForm></CategoriesForm> */}
      </OrderContextProvider>
    </div>
  );
};

export default ManageOrders;
