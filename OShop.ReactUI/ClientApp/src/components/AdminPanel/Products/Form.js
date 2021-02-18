import React, { Component } from "react";
import axios from "axios";

export default class Form extends Component {
  constructor(props) {
    super(props);
    this.myChangeHandler = this.myChangeHandler.bind(this);
    this.myChangeHandlerFiles = this.myChangeHandlerFiles.bind(this);
    this.postContent = this.postContent.bind(this);
    this.updateContent = this.updateContent.bind(this);
  }

  myChangeHandler = event => {
    let nam = event.target.name;
    let val = event.target.value;
    this.props.passHandler(nam, val);
  };

  myChangeHandlerFiles = event => {
    const file = event.target.files[0];
    if (
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

  async postContent(pagenmbr) {
    this.props.passLoading();
    const form = new FormData();
    form.append("name", this.props.counterStates.name);
    form.append("description", this.props.counterStates.description);
    form.append("stock", this.props.counterStates.stock);
    form.append("price", this.props.counterStates.price);
    form.append("photo", this.props.counterStates.photo);
    form.append("categoryRefId", this.props.counterStates.categoryRefId);
    await axios
      .post("/AdminPanel/createproduct", form)
      .then(result => console.log(result))
      .catch(error => console.log(error));

    await this.props.passUpdPost(pagenmbr);
  }
  async updateContent(pagenmbr) {
    await this.props.passLoading();
    const form = new FormData();
    form.append("productId", this.props.counterStates.productId);
    form.append("name", this.props.counterStates.name);
    form.append("description", this.props.counterStates.description);
    form.append("stock", this.props.counterStates.stock);
    form.append("price", this.props.counterStates.price);
    form.append("photo", this.props.counterStates.photo);
    form.append("categoryRefId", this.props.counterStates.categoryRefId);

    await axios
      .put("/AdminPanel/updateproduct", form)
      .then(result => console.log(result))
      .catch(error => console.log(error));

    await this.props.passUpdPut(pagenmbr);
  }

  render() {
    var button = null;
    if (this.props.counterStates.productId === 0) {
      button = (
        <button
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          onClick={() => this.postContent(this.props.counterStates.currpage)}
        >
          Add Product
        </button>
      );
    } else {
      button = (
        <button
          className="btn btn-outline-primary btn-sm"
          style={{ float: "right", margin: 10 + "px", padding: 10 + "px" }}
          onClick={() => this.updateContent(this.props.counterStates.currpage)}
        >
          Update
        </button>
      );
    }
    return (
      <div>
        <table className="table table-striped" aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Product</th>
              <th>Input</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <th>Name</th>
              <td className="form-group">
                <input
                  type="text"
                  name="name"
                  value={this.props.counterStates.name}
                  onChange={this.myChangeHandler}
                  className="form-control"
                />
              </td>
            </tr>
            <tr>
              <th>Description</th>
              <td className="form-group">
                <input
                  type="text"
                  name="description"
                  value={this.props.counterStates.description}
                  onChange={this.myChangeHandler}
                  className="form-control"
                />
              </td>
            </tr>
            <tr>
              <th>Stock</th>
              <td className="form-group">
                <input
                  type="number"
                  name="stock"
                  min="0"
                  max="10000"
                  value={this.props.counterStates.stock}
                  onChange={this.myChangeHandler}
                  className="form-control"
                />
              </td>
            </tr>
            <tr>
              <th>Price</th>
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
                  {this.props.counterStates.categories.map(category => (
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
        </table>
        {button}
      </div>
    );
  }
}
