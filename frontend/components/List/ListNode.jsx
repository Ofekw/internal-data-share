import React from 'react';
import List from './List.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import { Router, Route, Link } from 'react-router';

var ListContainer = React.createClass({

	/*if parent == "" // then root, get top level
		this.state.childNodes == ajax call to get top level
	else
		this.state.childNodes = ajax call get children of (this.props.parent) */

	getChildrenNodes: function(){
		if(!this.props.parent){
			var children = { //This would be the async call to get children of root
					data : 'cardDataObject',
					childNodes : ["Bank1", "Bank2", "Bank3"]
				}
		}else{
			//Get children of parent
			var children = { //This would be the async call to get children of parent
				data : 'cardDataObject',
				childNodes : ["VM1", "VM2", "VM3"]
			}
		}
		return children;
	},

	getInitialState: function() {
		return this.getChildrenNodes();
	},

	handleClick: function (item){
		var scope = this;
		this.props.handleClick(item,function(){
			scope.setState(scope.getChildrenNodes());
		});	
	},

	render: function(){
		return (
			<Card>
				<List listItems={this.state.childNodes} handleClick={this.handleClick}></List>
			</Card>
		)
	}
});


export default ListContainer;