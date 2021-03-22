import React, { useContext } from "react";
import { Button } from "reactstrap";
import { OrderContext } from "../../../contexts/OrderContext";

const OrdersTable = () => {
  const { orders, getOrderInfo, toggleModal } = useContext(OrderContext);

  const showOrderInfo = (orderId) => {
    getOrderInfo(orderId);
    toggleModal();
  };

  return (
    <div>
      <table
        className="table table-striped"
        style={{
          textAlign: "center",
          border: 2 + "px solid black",
        }}
      >
        <thead>
          <tr>
            <th style={{ textAlign: "left" }}>OrderId</th>
            <th>Status</th>
            <th>Total</th>
            <th>Date Created</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.orderId}>
              <td style={{ textAlign: "left" }}>{order.orderId}</td>
              <td>{order.status}</td>
              <td>{order.totalOrdered}</td>
              <td>{order.created.slice(0, 10)}</td>
              <td>
                <Button
                  color="warning"
                  onClick={() => showOrderInfo(order.orderId)}
                >
                  Order Info
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default OrdersTable;
