import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import Delete from 'material-ui/svg-icons/action/delete';
import Undo from 'material-ui/svg-icons/content/undo';
import RaisedButton from 'material-ui/RaisedButton';
import $ from 'jquery';
import config from '../../config.js';

// Component that renders the List Items view.
var ListComponent = React.createClass({

	// Tells the parent component that a list item has been clicked
	onClick: function (item){
		if (this.props.editable){
			return
		}
		this.props.handleClick(item);
	},

	// Marks items that are going to be deleted
	onDeleteItem: function(id){
		var del = this.state.delete;
		if (del.indexOf(id) > -1){
			del.splice(del.indexOf(id), 1);
		}
		else{
			del.push(id);
		}
		this.setState({delete: del});
	},

	// Sets the initial state of component
	getInitialState: function(){
		return {delete: []}
	},

	render: function(){
		var buttonStyle = {
			float: 'right',
			marginTop: 5 
		};

		// Deletes items that are marked as deleted.
		if (!this.props.editable && this.state.delete.length !== 0){
			this.state.delete.forEach(function(e){
				$.delete(config.apiHost+'items/' + e);
			})
		}
		return (
			<List>
				<Subheader>Children Nodes</Subheader>
				{
					this.props.listItems.map( item => {
						// Toggles background colour if an item is going to be deleted
						var icon = <Delete />;
						var style={};
						if (this.state.delete.indexOf(item.Id) > -1){
							icon = <Undo />
							style = {backgroundColor: '#ddd'}
						}
						return <ListItem primaryText={item.Key} key={item.Id} disabled={this.props.editable} onClick={this.onClick.bind(this,item)} style={style}
									rightIconButton={this.props.editable ? <RaisedButton icon={icon} style={buttonStyle} onTouchTap={this.onDeleteItem.bind(this, item.Id)}/> : null
								}/>
					})
				}
			</List>
		)
	}
});


export default ListComponent;