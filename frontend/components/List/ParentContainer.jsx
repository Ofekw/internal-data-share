import React from 'react';
import ListNode from './ListNode.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import { Router, Route, Link } from 'react-router';
import Breadcrumbs from 'react-breadcrumbs';


var ParentContainer = React.createClass({

	getInitialState: function() {
		return {
			parent: null
		}
	},

	handleClick: function(item,cb) {
		this.setState({parent: item}, function(){
			console.log(this.state.parent);
			cb();
		});
	},

	render: function(){
		return (
			<div>
				<h1>ParentContainter</h1>
				<Breadcrumbs routes={this.props.routes} params={this.props.params} setDocumentTitle={true}/>
			 	<ListNode parent={this.state.parent} handleClick={this.handleClick}/>
		 	</div>
		)
	}
});


export default ParentContainer;