import React, { Component } from 'react';
import axios from 'axios';

export class FetchData extends Component {
    static displayName = FetchData.name;
    constructor(props) {
        super(props);
        this.state = { products: [], categories: [], loading: true };
        this.forceUpdate = this.forceUpdate.bind(this);
    }
    

    forceUpdate() {
        axios.get('/AdminPanel/getallproducts').then(response => { this.setState({ products: response.data }); console.log(this.state.products); });
        axios.get('/AdminPanel/getallcategories').then(response => { this.setState({ categories: response.data, loading: false }); console.log(this.state.categories); });
    }
    componentDidMount() {
        axios.get('/AdminPanel/getallproducts').then(response => { this.setState({ products: response.data }); console.log(this.state.products); });
        axios.get('/AdminPanel/getallcategories').then(response => { this.setState({ categories: response.data, loading: false }); console.log(this.state.categories); });
    };

    static renderForecastsTable(products, categories) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Category</th>
                        <th>Descript</th>
                        <th>Stock</th>
                        <th>Price</th>
                        <th>Photo</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map(product =>
                        <tr key={product.productId}>
                            <td>{product.name}</td>
                            <td>{categories.filter(category => category.categoryId === product.categoryRefId)[0].name}</td>
                            <td>{product.description}</td>
                            <td>{product.stock}</td>
                            <td>{product.price}</td>
                            <td><img src={`WebImage/GetImage/${product.photo}`} style={{ width: 50 + 'px', height: 50 + 'px', objectFit: "cover" }} alt="productphoto"/></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    };

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.products, this.state.categories);

        return (
            <div>
                <button onClick={this.forceUpdate}>Update</button>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    };
    
};
