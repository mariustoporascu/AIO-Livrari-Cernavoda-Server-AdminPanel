import React, { Component } from "react";
import Form from "./Form";
import ProductsTable from "./ProductsTable";
import axios from "axios";
import Pagination from "../../Pagination";

export class ManageProducts extends Component {
  static displayName = ManageProducts.name;
  constructor(props) {
    super(props);
    this.state = {
      productId: 0,
      name: "",
      description: "",
      stock: 0,
      price: 0.01,
      photo: [],
      categoryRefId: 0,
      categories: [],
      itemsArray: [],
      findArray: [],
      pageItems: [],
      totalPages: [],
      currPage: null,
      loadingForm: true,
      loadingItems: true
    };
    this.removeProduct = this.removeProduct.bind(this);
    this.updateAfterPost = this.updateAfterPost.bind(this);
    this.updateAfterPut = this.updateAfterPut.bind(this);
    this.changePage = this.changePage.bind(this);
    this.find = this.find.bind(this);
  }

  async componentDidMount() {
    await axios
      .get("/AdminPanel/getallproducts")
      .then(response => {
        this.setState({ itemsArray: response.data });
      })
      .catch(error => console.log(error));
    await axios
      .get("/AdminPanel/getallcategories")
      .then(response => {
        this.setState({
          categories: response.data,
          categoryRefId: response.data[0].categoryId,
          loadingForm: false,
          loadingItems: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    var pagination = Pagination.pagination(1, this.state.itemsArray, 5);
    await this.setState({
      currPage: 1,
      pageItems: pagination.filter,
      loadingItems: false
    });
  }

  handleChange = (nam, val) => {
    this.setState({ [nam]: val });
  };

  handleLoading = () => {
    this.setState({ loadingForm: true, loadingItems: true });
  };

  handleProduct = product => {
    this.setState({
      productId: product.productId,
      name: product.name,
      description: product.description,
      stock: product.stock,
      price: product.price,
      categoryRefId: product.categoryRefId
    });
  };
  async changePage(pageNmbr) {
    var pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    await this.setState({ currPage: pageNmbr, pageItems: pagination.filter });
  }

  async removeProduct(name) {
    await this.setState({ loadingItems: true });
    await axios
      .delete("/AdminPanel/deleteproduct/" + name)
      .then(response => console.log(response))
      .catch(error => console.log(error));
    await axios
      .get("/AdminPanel/getallproducts")
      .then(response => {
        this.setState({
          itemsArray: response.data
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    if (this.state.totalPages.length < this.state.currPage) {
      let pagination = Pagination.pagination(
        this.state.currPage - 1,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      let pagination = Pagination.pagination(
        this.state.currPage,
        this.state.itemsArray,
        5
      );
      await this.setState({
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  async updateAfterPost(pageNmbr) {
    await this.setState({
      productId: 0,
      name: "",
      description: "",
      stock: 0,
      price: 0.01,
      photo: [],
      categoryRefId: this.state.categories[0].categoryId
    });
    await axios
      .get("/AdminPanel/getallproducts")
      .then(response => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    if (this.state.totalPages.length > pageNmbr) {
      let pagination = Pagination.pagination(
        this.state.totalPages.length,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      let pagination = Pagination.pagination(
        pageNmbr,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  async updateAfterPut(pageNmbr) {
    await this.setState({
      productId: 0,
      name: "",
      description: "",
      stock: 0,
      price: 0.01,
      photo: [],
      categoryRefId: this.state.categories[0].categoryId
    });
    await axios
      .get("/AdminPanel/getallproducts")
      .then(response => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    let pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    await this.setState({
      currPage: pagination.pageNmbr,
      pageItems: pagination.filter,
      loadingItems: false
    });
  }
  async find(event) {
    var value = event.target.value;
    await this.setState({ loadingItems: true });
    if (!!value) {
      await this.setState({
        findArray: this.state.itemsArray.filter(product =>
          product.name.toLowerCase().includes(value.toLowerCase())
        )
      });
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.findArray, 5)
      });
      let pagination = Pagination.pagination(1, this.state.findArray, 5);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
      });
      let pagination = Pagination.pagination(1, this.state.itemsArray, 5);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  render() {
    let productContents = this.state.loadingItems ? (
      <p>
        <em>Loading products...</em>
      </p>
    ) : (
      <ProductsTable
        counterStates={this.state}
        passEditProduct={this.handleProduct}
        passRemoveProduct={this.removeProduct}
      />
    );
    let formContents = this.state.loadingForm ? (
      <p>
        <em>refreshing form...</em>
      </p>
    ) : (
      <Form
        passHandler={this.handleChange}
        passLoading={this.handleLoading}
        counterStates={this.state}
        passUpdPost={this.updateAfterPost}
        passUpdPut={this.updateAfterPut}
      />
    );
    let pagination = (
      <Pagination counterStates={this.state} passChangePage={this.changePage} />
    );
    return (
      <div>
        <h1 id="tabelLabel">Product List</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {formContents}
        <input
          type="text"
          name="find"
          placeholder="Search product"
          onChange={this.find}
        />
        {productContents}
        {pagination}
      </div>
    );
  }
}
