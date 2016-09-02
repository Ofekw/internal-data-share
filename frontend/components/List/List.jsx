import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import Delete from 'material-ui/svg-icons/action/delete';
import Undo from 'material-ui/svg-icons/content/undo';
import RaisedButton from 'material-ui/RaisedButton';

var ListComponent = React.createClass({

	onClick: function (item){
		if (this.props.editable){
			return
		}
		this.props.handleClick(item);
	},

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

	getInitialState: function(){
		return {delete: []}
	},

	render: function(){
		var buttonStyle = {
			float: 'right',
			marginTop: 5 
		};

		return (
			<List>
				<Subheader>Children Nodes</Subheader>
				{
					this.props.listItems.map( item => {
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