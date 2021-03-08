/* eslint react/no-multi-comp: 0, react/prop-types: 0 */

import axios from "axios";
import React, { Component } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import Loading from "../loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();
export default class OrderInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
      orderInfoId: 0,
      firstName: "",
      lastName: "",
      address: "",
      phoneNo: "",
      disabledBtn: true,
      orderRefId: 0,
      data: [],
      loading: true,
    };
  }

  async componentDidMount() {
    await axios
      .get("ShoppingCart/orderinfo/" + this.props.customer)
      .then((result) => this.setState({ data: result.data }))
      .catch((error) => console.log(error));
    if (!!this.state.data.orderInfoId) {
      this.setState({
        orderInfoId: this.state.data.orderInfoId,
        firstName: this.state.data.firstName,
        lastName: this.state.data.lastName,
        address: this.state.data.address,
        phoneNo: this.state.data.phoneNo,
        orderRefId: this.state.data.orderRefId,
        loading: false,
      });
    } else {
      this.setState({
        orderRefId: this.state.data.orderRefId,
        loading: false,
      });
    }
    if (
      !!this.state.firstName &&
      !!this.state.lastName &&
      !!this.state.address &&
      !!this.state.phoneNo
    ) {
      this.setState({ disabledBtn: false });
    }
  }
  setValues = (event) => {
    event.persist();
    let name = event.target.name;
    let value = event.target.value;
    this.setState({ [name]: value });
    this.setCustomValidity(event);
  };

  setCustomValidity = (event) => {
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

  toggle = () => this.setState({ modal: !this.state.modal });

  postOrderInfo = async (event) => {
    event.persist();
    event.preventDefault();
    const form = new FormData();
    form.append("firstName", this.state.firstName);
    form.append("lastName", this.state.lastName);
    form.append("address", this.state.address);
    form.append("phoneNo", this.state.phoneNo);
    form.append("orderRefId", this.state.orderRefId);
    if (this.state.orderInfoId === 0) {
      await axios
        .post("ShoppingCart/addorderinfo", form)
        .then(() => toast.success("Saved", { autoClose: 2000 }))
        .catch((error) => console.log(error));
    } else {
      form.append("orderInfoId", this.state.orderInfoId);
      await axios
        .put("ShoppingCart/updateorderinfo", form)
        .then(() => toast.success("Saved", { autoClose: 2000 }))
        .catch((error) => console.log(error));
    }
    this.setState({ disabledBtn: false });
    this.toggle();
  };

  render() {
    let showSpan = this.state.disabledBtn ? (
      <span
        className="d-inline-block"
        tabIndex="0"
        data-toggle="tooltip"
        title="You must add order info to proceed"
        style={{
          float: "right",
        }}
      >
        <a
          className="btn btn-warning disabled"
          style={{ margin: 1 + "em", float: "right" }}
          href="/checkout"
        >
          Checkout
        </a>
      </span>
    ) : (
      <a
        className="btn btn-warning"
        style={{ margin: 1 + "em", float: "right" }}
        href="/checkout"
      >
        Checkout
      </a>
    );
    let modalWindow = this.state.loading ? (
      <Loading />
    ) : (
      <div>
        {showSpan}
        <Button
          color="primary"
          style={{ margin: 1 + "em", float: "right" }}
          onClick={this.toggle}
        >
          Add Order Info
        </Button>

        <Modal
          isOpen={this.state.modal}
          toggle={this.toggle}
          className="OrderInfo"
        >
          <ModalHeader toggle={this.toggle}>Order Info</ModalHeader>
          <form onSubmit={this.postOrderInfo}>
            <ModalBody>
              <input
                name="orderInfoId"
                value={this.state.orderInfoId}
                type="hidden"
              />
              <table className="table table-striped">
                <thead></thead>
                <tbody>
                  <tr>
                    <th>* First Name</th>
                    <td>
                      <input
                        type="text"
                        name="firstName"
                        value={this.state.firstName}
                        onChange={this.setValues}
                        required
                        className="form-control"
                        onInvalid={this.setCustomValidity}
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
                        value={this.state.lastName}
                        onChange={this.setValues}
                        required
                        className="form-control"
                        onInvalid={this.setCustomValidity}
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
                        value={this.state.address}
                        onChange={this.setValues}
                        required
                        className="form-control"
                        onInvalid={this.setCustomValidity}
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
                        value={this.state.phoneNo}
                        onChange={this.setValues}
                        required
                        className="form-control"
                        onInvalid={this.setCustomValidity}
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
              <Button color="secondary" onClick={this.toggle}>
                Cancel
              </Button>
            </ModalFooter>
          </form>
        </Modal>
      </div>
    );
    return <div>{modalWindow}</div>;
  }
}
