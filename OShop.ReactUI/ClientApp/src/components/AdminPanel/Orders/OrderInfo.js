import React, { useContext } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import { OrderContext } from "../../../contexts/OrderContext";

const OrderInfo = () => {
  const {
    modal,
    products,
    orderInfo,
    productsInOrder,
    toggleModal,
    clearData,
  } = useContext(OrderContext);

  const clearInfo = () => {
    clearData();
    toggleModal();
  };

  return (
    <div>
      <Modal isOpen={modal} toggle={clearInfo} className="OrderInfo">
        <ModalHeader toggle={clearInfo}>Order Info</ModalHeader>

        <ModalBody>
          {orderInfo.length !== 0 &&
          productsInOrder.length !== 0 &&
          products.length !== 0 ? (
            <div>
              <table className="table table-striped">
                <thead></thead>

                <tbody>
                  <tr>
                    <th>First Name</th>
                    <td>{orderInfo.firstName}</td>
                  </tr>
                  <tr>
                    <th>Last Name</th>
                    <td>{orderInfo.lastName}</td>
                  </tr>
                  <tr>
                    <th>Address</th>
                    <td>{orderInfo.address}</td>
                  </tr>
                  <tr>
                    <th>Phone No</th>
                    <td>{orderInfo.phoneNo}</td>
                  </tr>
                </tbody>
              </table>
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
                  {products.map((product) => (
                    <tr key={product.productId}>
                      <td>{product.name}</td>
                      <td>
                        {
                          productsInOrder.filter(
                            (item) => item.productRefId === product.productId
                          )[0].usedQuantity
                        }
                      </td>
                      <td>
                        {(
                          product.price *
                          productsInOrder.filter(
                            (item) => item.productRefId === product.productId
                          )[0].usedQuantity
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
              </table>
            </div>
          ) : null}
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={clearInfo}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
    </div>
  );
};

export default OrderInfo;
