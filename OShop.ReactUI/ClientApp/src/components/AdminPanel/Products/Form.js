import React, { Component } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();

export default class Form extends Component {
  myChangeHandler = (event) => {
    let nam = event.target.name;
    let val = event.target.value;
    this.props.passHandler(nam, val);
    if (nam !== "categoryRefId" && nam !== "photo") {
      this.setCustomValidity(event);
    }
  };

  myChangeHandlerFiles = (event) => {
    event.preventDefault();
    const file = event.target.files[0];
    if (
      !!file &&
      file.type !== "image/png" &&
      file.type !== "image/jpg" &&
      file.type !== "image/jpeg"
    ) {
      alert("You selected unaccepted format");
      event.target.files = null;
    } else {
      this.props.passHandler("photo", file);
    }
  };

  postContent = async (event) => {
    event.persist();
    event.preventDefault();
    this.props.passLoading();
    const form = new FormData();
    form.append("name", this.props.counterStates.name);
    form.append("description", this.props.counterStates.description);
    form.append("stock", this.props.counterStates.stock);
    form.append("price", this.props.counterStates.price);
    form.append("photo", this.props.counterStates.photo);
    form.append("categoryRefId", this.props.counterStates.categoryRefId);
    if (this.props.counterStates.productId === 0) {
      await axios
        .post("/AdminPanel/createproduct", form)
        .then(() => toast.success("Added", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));

      await this.props.passUpdPost(this.props.counterStates.currPage);
    } else {
      form.append("productId", this.props.counterStates.productId);

      await axios
        .put("/AdminPanel/updateproduct", form)
        .then(() => toast.success("Updated", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));

      await this.props.passUpdPut(this.props.counterStates.currPage);
    }
  };
  setCustomValidity = (event) => {
    event.preventDefault();
    if (event.target.value === "") {
      if (event.target.name === "name" || event.target.name === "description") {
        document.getElementById("span " + event.target.name).textContent =
          "This cannot be empty";
      } else {
        document.getElementById("span " + event.target.name).textContent =
          "Value must be a number between " +
          event.target.min +
          " and " +
          event.target.max;
      }
    } else {
      document.getElementById("span " + event.target.name).textContent = "";
    }
  };

  render() {
    var button = null;
    if (this.props.counterStates.productId === 0) {
      button = (
        <button
          type="submit"
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          //onClick={() => this.postContent(this.props.counterStates.currPage)}
        >
          Add Product
        </button>
      );
    } else {
      button = (
        <button
          type="submit"
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          //onClick={() => this.updateContent(this.props.counterStates.currPage)}
        >
          Update
        </button>
      );
    }
    return (
      <div>
        <form onSubmit={this.postContent}>
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th>Product</th>
                <th>Input</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <th>* Name</th>
                <td className="form-group">
                  <input
                    type="text"
                    name="name"
                    value={this.props.counterStates.name}
                    onChange={this.myChangeHandler}
                    className="form-control"
                    required
                    onInvalid={this.setCustomValidity}
                  />
                  <span
                    id="span name"
                    className="text-danger field-validation-valid"
                  />
                </td>
              </tr>
              <tr>
                <th>* Description</th>
                <td className="form-group">
                  <input
                    type="text"
                    name="description"
                    value={this.props.counterStates.description}
                    onChange={this.myChangeHandler}
                    className="form-control"
                    required
                    onInvalid={this.setCustomValidity}
                  />
                  <span
                    id="span description"
                    className="text-danger field-validation-valid"
                  />
                </td>
              </tr>
              <tr>
                <th>* Stock</th>
                <td className="form-group">
                  <input
                    type="number"
                    name="stock"
                    min="0"
                    max="10000"
                    value={this.props.counterStates.stock}
                    onChange={this.myChangeHandler}
                    className="form-control"
                    required
                    onInvalid={this.setCustomValidity}
                  />
                  <span
                    id="span stock"
                    className="text-danger field-validation-valid"
                  />
                </td>
              </tr>
              <tr>
                <th>* Price</th>
                <td className="form-group">
                  <input
                    type="number"
                    step={0.01}
                    min="0.01"
                    max="10000"
                    name="price"
                    value={this.props.counterStates.price}
                    onChange={this.myChangeHandler}
                    className="form-control"
                    required
                    onInvalid={this.setCustomValidity}
                  />
                  <span
                    id="span price"
                    className="text-danger field-validation-valid"
                  />
                </td>
              </tr>
              <tr>
                <th>Photo</th>
                <td className="form-group">
                  <input
                    type="file"
                    accept="image/png,image/jpg,image/jpeg"
                    name="photo"
                    onChange={this.myChangeHandlerFiles}
                  />
                </td>
              </tr>
              <tr>
                <th>Category</th>
                <td className="form-group">
                  <select
                    name="categoryRefId"
                    value={this.props.counterStates.categoryRefId}
                    onChange={this.myChangeHandler}
                    className="form-control"
                  >
                    {this.props.counterStates.categories.map((category) => (
                      <option
                        key={category.categoryId}
                        value={category.categoryId}
                      >
                        {category.name}
                      </option>
                    ))}
                  </select>
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <th>* Required</th>
              </tr>
            </tfoot>
          </table>
          {button}
        </form>
      </div>
    );
  }
}
