/*eslint unicode-bom: ["error", "always"]*/
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
    this.setCustomValidity(event);
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
    } else {
      this.props.passHandler("photo", file);
    }
  };

  postContent = async (event) => {
    event.persist();
    event.preventDefault();
    this.props.passLoading();
    const form = new FormData();
    if (this.props.counterStates.categoryId === 0) {
      form.append("name", this.props.counterStates.name);
      form.append("photo", this.props.counterStates.photo);
      await axios
        .post("/AdminPanel/createcategory", form)
        .then(() => toast.success("Added", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));

      await this.props.passUpdPost(this.props.counterStates.currPage);
    } else {
      form.append("categoryId", this.props.counterStates.categoryId);
      form.append("name", this.props.counterStates.name);
      form.append("photo", this.props.counterStates.photo);

      await axios
        .put("/AdminPanel/updatecategory", form)
        .then(() => toast.success("Updated", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));

      await this.props.passUpdPut(this.props.counterStates.currPage);
    }
  };
  setCustomValidity = (event) => {
    event.preventDefault();
    if (event.target.value === "") {
      document.getElementById("span " + event.target.name).textContent =
        "This cannot be empty";
    } else {
      document.getElementById("span " + event.target.name).textContent = "";
    }
  };

  render() {
    var button = null;
    if (this.props.counterStates.categoryId === 0) {
      button = (
        <button
          type="submit"
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          //onClick={this.postContent}
        >
          Add Category
        </button>
      );
    } else {
      button = (
        <button
          type="submit"
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          //onClick={this.updateContent}
        >
          Update
        </button>
      );
    }
    return (
      <div>
        <form onSubmit={this.postContent}>
          <input
            name="categoryId"
            value={this.props.counterStates.categoryId}
            type="hidden"
          />
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th>Category</th>
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
