import React from 'react';
import List from './List.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import $ from 'jquery';
import config from '../../config.js';
import TextField from 'material-ui/TextField';
import Divider from 'material-ui/Divider';
import IconButton from 'material-ui/IconButton';
import FlatButton from 'material-ui/FlatButton';
import AddIcon from 'material-ui/svg-icons/content/add-box';

// Component that renders List Items view and ability to add items to the List
var ListContainer = React.createClass({

	// Set up initial state
	getInitialState: function() {
		return { inputText:"" };
	},

	// Tells parent container that a list item has been clicked
	handleClick: function (item,breadcrumbFlag){
		this.props.handleClick(item,breadcrumbFlag);
	},

	// Event for changing text field
	textChange: function(event){
		this.setState({inputText:event.target.value})
	},

	// Handles the new addition of an list item
	addNewNode: function () {
		// Gets the name of the list item
		var text = $('#newListField').val();
		// Show error if no name is provided
		if (text === ""){
			this.setState({ errors: "This field is required"});
		// Collect the data and posts it to the database
		} else {
			var self = this;
			var data = {
				Key: text,
				Type: 'node'
			};
			if (this.props.parent) {
				data.Parent = this.props.parent.Id;
			}
			$.post({
				url: config.apiHost + 'items',
				data: JSON.stringify(data),
				success: function(result) {
					self.handleClick(result,true);
					self.setState({inputText:""});
				},
				headers: {
					'Content-Type': 'application/json'
				}
			});
		}
	},

	render: function () {
		var divStyle = {
			display: 'flex',
		};

		var itemStyle = {
			marginLeft: 10,
			width: '100%',
			display: 'inline-block',
			position: 'relative'
		};

		var buttonStyle = {
			display: 'inline-block',
			position: 'relative',
			width: '150px'
		};


		var addItem;
		if (this.props.editable) {
			addItem =
				<div style={divStyle}>
					<TextField id="newListField" style={itemStyle} errorText={this.state.errors} hintText="Node Name"/>
					<FlatButton label="Add Node" style={buttonStyle} primary={true} onTouchTap={this.addNewNode} />
				</div>
		}

		return (
			<Card>
				<List listItems={this.props.nodes} handleClick={this.handleClick} editable={this.props.editable} parent={this.props.parent}></List>
				<Divider />
				{addItem}
			</Card>
		)
	}
});


export default ListContainer;
