import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Subheader from 'material-ui/Subheader';
import DeleteIcon from 'material-ui/svg-icons/action/delete';

var ListComponent = React.createClass({

	onClick: function (item){
		this.props.handleClick(item);
	},

	render: function(){
		var removeIcon;
		if(this.props.editable){
			removeIcon = <DeleteIcon/>;
		} else {
			removeIcon = null;
		}
		return (
			<List>
				<Subheader>Children Nodes</Subheader>
				{
					this.props.listItems.map( item => {
						return <ListItem primaryText={item.Key} key={item.Id} onClick={this.onClick.bind(this,item)} rightIconButton={removeIcon}/>
					})
				}
			</List>
		)
	}
});


export default ListComponent;