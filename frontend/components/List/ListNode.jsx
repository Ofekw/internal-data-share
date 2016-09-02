import React from 'react';
import List from './List.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import $ from 'jquery';
import config from '../../config.js';
import Info from '../Card.jsx';
import TextField from 'material-ui/TextField';
import Divider from 'material-ui/Divider';
import IconButton from 'material-ui/IconButton';
import AddIcon from 'material-ui/svg-icons/content/add-box';

var ListContainer = React.createClass({

	/*if parent == "" // then root, get top level
		this.state.childNodes == ajax call to get top level
	else
		this.state.childNodes = ajax call get children of (this.props.parent) */

	getChildrenNodes: function(){
		if(!this.props.parent){
			var self = this;
			$.get(config.apiHost+'GetApiItems.json', function (result) {
				self.setState({nodes : result});
			});
		}else{
			var self = this;
			$.get(config.apiHost+'GetApiItemsById.json', function (result) {
				//+self.props.parent.Id
				self.setState({nodes:result.NodeChildren});
			});
		}
	},

	getInitialState: function() {
		return {
			nodes:[]
		}	
	},

	handleClick: function (item){
		var self = this;
		this.props.handleClick(item);
	},

	handleTouchTap: function(){
		var text = $('#newListField').val();
		if (text === ""){
			this.setState({ errors: "This field is required"});
		}
		else{
			//TODO Post new field
		}
	},

	render: function(){
		this.getChildrenNodes();

		var textFieldStyle = {
			marginLeft: 10,
			width: "90%"
		};
		var iconButtonStyle = {
			float: 'right'
		};

		return (
			<Card>
			//TODO pass node object to card to be populated
				<List listItems={this.state.nodes} handleClick={this.handleClick} editable={this.props.editable}></List>
				<Divider />
				<div>
					<TextField id="newListField" style={textFieldStyle} errorText={this.state.errors} hintText="Hint Text"/>
					<IconButton label="Add" style={iconButtonStyle} onTouchTap={this.handleTouchTap}> <AddIcon/></IconButton>
				</div>
			</Card>
		)
	}
});


export default ListContainer;