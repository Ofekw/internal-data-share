import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import Delete from 'material-ui/svg-icons/action/delete';
import Undo from 'material-ui/svg-icons/content/undo';
import RaisedButton from 'material-ui/RaisedButton';
import Chip from 'material-ui/Chip'
import $ from 'jquery';
import config from '../../config.js';
import async from '../../node_modules/async/dist/async.min.js';

const styles = {
	wrapper: {
		display: 'flex'
	},
	chip: {
		margin: '4px',
		display: 'inline-block'
	}
};

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

	componentDidMount: function(){
		this.setState({delete:[]});
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

			this.state.delete.forEach(function(e){
				var element = e;
				functions.push(function(cb){
					$.ajax({
						url: config.apiHost + "Items/" + element,
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
				{
					this.props.listItems.map( item => {
						// Toggles background colour if an item is going to be deleted
						var icon = <Delete />;
						var style={};
						if (this.state.delete.indexOf(item.Id) > -1){
							icon = <Undo />
							style = {backgroundColor: '#ddd'}
						}

						var labels = [];

						for (var label in item.Labels) {
							const labelElement = item.Labels[label];
							labels.push(
								<Chip
									key={labelElement.Id}
									style={styles.chip}
								>
									{labelElement.Content}
								</Chip>
							)
						}

						return (
							<div style={styles.wrapper} onClick={this.onClick.bind(this,item)}>
								<ListItem primaryText={item.Key} key={item.Id} disabled={this.props.editable} style={style}
									rightIconButton={this.props.editable ? <RaisedButton icon={icon} style={buttonStyle} onTouchTap={this.onDeleteItem.bind(this, item.Id)}/> : null}
									/>
									<span>
										{labels.map(function (label, index) {
											// Add the labels
											return label;
										})}
									</span>
							</div>
						)
					})
				}
			</List>
		)
	}
});



export default ListComponent;
