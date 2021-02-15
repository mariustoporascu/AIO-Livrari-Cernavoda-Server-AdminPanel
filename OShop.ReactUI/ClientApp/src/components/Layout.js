import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import { ManageProducts } from './ManageProducts';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <NavMenu />

                <div>
                    <ManageProducts />
                    
                </div>
            </div>
        );
    }
}
