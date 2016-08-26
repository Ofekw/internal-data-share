import React from 'react';
import List from './List.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import $ from 'jquery';
import config from '../../config.js';
import Info from '../Card.jsx'

var ListContainer = React.createClass({

	/*if parent == "" // then root, get top level
		this.state.childNodes == ajax call to get top level
	else
		this.state.childNodes = ajax call get children of (this.props.parent) */

	getChildrenNodes: function(){
		var state = {
			nodes : []
		};
		if(!this.props.parent){
			var self = this;
			$.get(config.apiHost+'/api/items', function (result) {
				self.setState({nodes : result});
			});
		}else{
			var self = this;
			$.get(config.apiHost+'/api/items/'+self.props.parent.Id, function (result) {
				self.setState({nodes:result.Children});
			});
		}
		return state;
	},

	getInitialState: function() {
		return this.getChildrenNodes();
	},

	handleClick: function (item){
		var self = this;
		this.props.handleClick(item,function(){
			self.getChildrenNodes();
		});
	},

	render: function(){
		
		return (
			<Card>
			//TODO pass node object to card to be populated
				<List listItems={this.state.nodes} handleClick={this.handleClick}></List>
			</Card>
		)
	}
});


export default ListContainer;