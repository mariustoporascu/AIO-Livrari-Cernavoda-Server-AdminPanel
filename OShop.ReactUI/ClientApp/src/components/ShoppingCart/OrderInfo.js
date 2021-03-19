/* eslint react/no-multi-comp: 0, react/prop-types: 0 */

import axios from "axios";
import React, { Component, useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import { CartContext } from "../../contexts/CartContext";
import { DataContext } from "../../contexts/DataContext";

const OrderInfo = () => {
  const {
    orderInfoId,
    firstName,
    lastName,
    address,
    phoneNo,
    orderRefId,
    disabledBtn,
    setDisabledBtn,
    setValues,
    setCustomValidity,
  } = useContext(CartContext);
  const { toast } = useContext(DataContext);
  const [modal, setModal] = useState(false);

  const toggleModal = () => {
    setModal(!modal);
  };

  const postOrderInfo = async (event) => {
    event.preventDefault();
    const form = new FormData();
    form.append("firstName", firstName);
    form.append("lastName", lastName);
    form.append("address", address);
    form.append("phoneNo", phoneNo);
    form.append("orderRefId", orderRefId);
    if (orderInfoId === 0) {
      await axios
        .post("ShoppingCart/addorderinfo", form)
        .then(() => toast.success("Saved", { autoClose: 2000 }))
        .catch((error) => console.log(error));
    } else {
      form.append("orderInfoId", orderInfoId);
      await axios
        .put("ShoppingCart/updateorderinfo", form)
        .then(() => toast.success("Saved", { autoClose: 2000 }))
        .catch((error) => console.log(error));
    }
    setDisabledBtn(false);
    toggleModal();
  };

  return (
    <div>
      {disabledBtn ? (
        <span
          className="d-inline-block"
          tabIndex="0"
          data-toggle="tooltip"
          title="You must add order info to proceed"
          style={{
            float: "right",
          }}
        >
          <Link
            tag={Link}
            className="btn btn-default disabled"
            style={{ margin: 1 + "em", float: "right" }}
            to="/checkout"
          >
            Checkout
          </Link>
        </span>
      ) : (
        <Link
          tag={Link}
          className="btn btn-info"
          style={{ margin: 1 + "em", float: "right" }}
          to="/checkout"
        >
          Checkout
        </Link>
      )}
      <Button
        color="warning"
        style={{ margin: 1 + "em", float: "right" }}
        onClick={toggleModal}
      >
        Add Order Info
      </Button>

      <Modal isOpen={modal} toggle={toggleModal} className="OrderInfo">
        <ModalHeader toggle={toggleModal}>Order Info</ModalHeader>
        <form onSubmit={postOrderInfo}>
          <ModalBody>
            <table className="table table-striped">
              <thead></thead>
              <tbody>
                <tr>
                  <th>* First Name</th>
                  <td>
                    <input
                      type="text"
                      name="firstName"
                      value={firstName}
                      onChange={setValues}
                      required
                      className="form-control"
                      onInvalid={setCustomValidity}
                    />
                    <span
                      id="span firstName"
                      className="text-danger field-validation-valid"
                    ></span>
                  </td>
                </tr>
                <tr>
                  <th>* Last Name</th>
                  <td>
                    <input
                      type="text"
                      name="lastName"
                      value={lastName}
                      onChange={setValues}
                      required
                      className="form-control"
                      onInvalid={setCustomValidity}
                    />
                    <span
                      id="span lastName"
                      className="text-danger field-validation-valid"
                    ></span>
                  </td>
                </tr>
                <tr>
                  <th>* Address</th>
                  <td>
                    <input
                      type="text"
                      name="address"
                      value={address}
                      onChange={setValues}
                      required
                      className="form-control"
                      onInvalid={setCustomValidity}
                    />
                    <span
                      id="span address"
                      className="text-danger field-validation-valid"
                    ></span>
                  </td>
                </tr>
                <tr>
                  <th>* Phone No</th>
                  <td>
                    <input
                      type="text"
                      name="phoneNo"
                      value={phoneNo}
                      onChange={setValues}
                      required
                      className="form-control"
                      onInvalid={setCustomValidity}
                    />
                    <span
                      id="span phoneNo"
                      className="text-danger field-validation-valid"
                    ></span>
                  </td>
                </tr>
              </tbody>
            </table>
          </ModalBody>
          <ModalFooter>
            <button type="submit" className="btn btn-primary">
              Save Info
            </button>
            <Button color="secondary" onClick={toggleModal}>
              Cancel
            </Button>
          </ModalFooter>
        </form>
      </Modal>
    </div>
  );
};

export default OrderInfo;
