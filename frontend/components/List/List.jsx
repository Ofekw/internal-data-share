import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import Delete from 'material-ui/svg-icons/action/delete';
import Undo from 'material-ui/svg-icons/content/undo';
import RaisedButton from 'material-ui/RaisedButton';
import $ from 'jquery';
import config from '../../config.js';
import async from '../../node_modules/async/dist/async.min.js';

// Component that renders the List Items view.
var ListComponent = React.createClass({

	// Tells the parent component that a list item has been clicked
	onClick: function (item,breadcrumbFlag){
		if (this.props.editable){
			return
		}
		this.props.handleClick(item,breadcrumbFlag);
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
		var self = this;

		// Deletes items that are marked as deleted.
		if (!this.props.editable && this.state.delete.length !== 0){
			var functions = [];

			// Sets up all the async calls for delete
			this.state.delete.forEach(function(e){
				functions.push(function(cb){
					$.ajax({
						url: config.apiHost + "Items/" + e,
						type: "DELETE",
						success: function(){
							cb();
						}
					});
				});
			});

			// Does all the async calls
			async.parallel(functions,function(){
				// Once all async calls are done, gets the object from the database again
				var parent = self.props.parent;
				self.onClick(parent,false);
			});
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