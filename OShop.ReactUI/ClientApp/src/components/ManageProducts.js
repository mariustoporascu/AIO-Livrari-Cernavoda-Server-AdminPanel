import React, { Component } from 'react';
import Form from './Form';
import ProductsTable from './ProductsTable';
import axios from "axios";

export class ManageProducts extends Component {
    static displayName = ManageProducts.name;
    constructor(props) {
        super(props);
        this.state = {
            productId: 0, name: "", description: "", stock: 0, price: 0.01, photo: [], categoryRefId: 7,
            products: [], categories: [], pageproducts: [], totalpages: [], currpage: null,
            loadingForm: true, loadingProducts: true
        };
        this.removeProduct = this.removeProduct.bind(this);
        this.updateAfterPost = this.updateAfterPost.bind(this);
        this.updateAfterPut = this.updateAfterPut.bind(this);
        this.pagination = this.pagination.bind(this);
    }

    async componentDidMount() {
        await axios.get('/AdminPanel/getallproducts').then(response => { this.setState({ products: response.data }); console.log(this.state.products); });
        await axios.get('/AdminPanel/getallcategories').then(response => {
            this.setState({ categories: response.data, loadingForm: false, loadingProducts: false });
            console.log(this.state.categories);
        });
        await this.pagination(1);
        await this.pageHelper();
    };

    handleChange = (nam, val) => {
        this.setState({ [nam]: val });
    }

    handleLoading = () => {
        this.setState({ loadingForm: true, loadingProducts: true });
    }

    handleProduct = (product) => {
        this.setState({
            productId: product.productId, name: product.name, description: product.description
            , stock: product.stock, price: product.price, categoryRefId: product.categoryRefId
        });
        console.log(product);
    }

    async removeProduct(productId) {
        await this.setState({ loadingForm: true, loadingProducts: true });
        await axios.delete('/AdminPanel/deleteproduct/' + productId).then(response => console.log(response))
            .catch(error => console.log(error));
        await axios.get('/AdminPanel/getallproducts')
            .then(response => {
                this.setState({ products: response.data, loadingForm: false, loadingProducts: false });
                console.log(this.state.products);
            });
        await this.pageHelper();
        if (this.state.totalpages.length < this.state.currpage)
            await this.pagination(this.state.currpage - 1);
        else
            await this.pagination(this.state.currpage);
    }

    async updateAfterPost(pagenmbr) {
        await this.setState({
            productId: 0, name: "", description: "", stock: 0, price: 0.01, photo: [], categoryRefId: 7,
        })
        await axios.get('/AdminPanel/getallproducts')
            .then(response => {
                this.setState({ products: response.data, loadingForm: false, loadingProducts: false });
                console.log(this.state.products);
            });
        await this.pageHelper();
        if (this.state.totalpages.length > pagenmbr)
            await this.pagination(this.state.totalpages.length);
        else
            await this.pagination(pagenmbr);
    }

    async updateAfterPut(pagenmbr) {
        await this.setState({
            productId: 0, name: "", description: "", stock: 0, price: 0.01, photo: [], categoryRefId: 7,
        })
        await axios.get('/AdminPanel/getallproducts')
            .then(response => {
                this.setState({ products: response.data, loadingForm: false, loadingProducts: false });
                console.log(this.state.products);
            });

        await this.pageHelper();
        await this.pagination(pagenmbr);
    }

    async pagination(pagenmbr) {
        var productnumber = pagenmbr * 5;
        var productsize = this.state.products.length;
        var i;
        var productsfilter = [];
        await this.setState({ loading: true });
        if (productnumber > productsize)
            for (i = productnumber - 5; i < productsize; i++) {
                productsfilter.push(this.state.products[i]);
            }
        else {
            for (i = productnumber - 5; i < productnumber; i++) {
                productsfilter.push(this.state.products[i]);
            }
        }
        await this.setState({ pageproducts: productsfilter, currpage: pagenmbr, loadingProducts: false, });
        console.log(this.state.pageproducts);
    }

    async pageHelper() {
        var totalproducts = this.state.products.length;
        var totpages = Math.ceil(totalproducts / 5);
        var i;
        var pages = [];
        for (i = 1; i < totpages + 1; i++) {

            pages.push(i)

        }
        await this.setState({ totalpages: pages })
        console.log(this.state.totalpages);
    }


    render() {
        let productContents = this.state.loadingProducts
            ? <p><em>Loading products...</em></p>
            : <ProductsTable counterStates={this.state}
                passEditProduct={this.handleProduct}
                passRemoveProduct={this.removeProduct} />;
        let formContents = this.state.loadingForm
            ? <p><em>refreshing form...</em></p>
            : <Form passHandler={this.handleChange}
                passLoading={this.handleLoading}
                counterStates={this.state}
                passUpdPost={this.updateAfterPost}
                passUpdPut={this.updateAfterPut} />;

        return (
            <div>
                <h1 id="tabelLabel" >Product List</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {formContents}
                {productContents}
                <div style={{ display: "flex", justifyContent: "center" }}>
                    {this.state.totalpages.map(page =>
                        <button key={page} className="btn btn-outline-success btn-sm" style={{ marginRight: 10 + "px" }} onClick={() => this.pagination(page)}>Page {page}</button>)
                    }
                </div>
            </div>
        )
    };
};