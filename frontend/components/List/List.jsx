import React from 'react';
import {List, ListItem} from 'material-ui/List';

var ListComponent = React.createClass({

	onClick: function (item){
		this.props.handleClick(item);
	},

	render: function(){
/*		if (this.props.listItems.length === 0){
			return (
				<div>Fuck You React You Piece of Shit"</div>
			)
		} else {*/
			return (
				<List>
					{
						this.props.listItems.map( item => {
							return <ListItem primaryText={item.Key} key={item.Id} onClick={this.onClick.bind(this,item)}/>
						})
					}
				</List>
			)
		//}
	}
});


export default ListComponent;