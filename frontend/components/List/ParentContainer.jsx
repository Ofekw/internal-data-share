import React from 'react';
import ListNode from './ListNode.jsx';
import Card from '../Card.jsx';
import FlatButton from 'material-ui/FlatButton';
import $ from 'jquery';
import config from '../../config.js';



var ParentContainer = React.createClass({

	getChildrenNodes: function(item,crumbs){
		if(!item){
			var self = this;
			$.get(config.apiHost+'items', function (result) {
				if(crumbs){
					self.setState({nodes : result, parent:null , breadcrumbs:crumbs});
				} else {
					self.setState({nodes : result, parent:null});
				}
			});
		}else{
			var self = this;
			$.get(config.apiHost+'items/' +item.Id, function (result) {
				if(crumbs){
					self.setState({nodes:result.NodeChildren, parent:result, breadcrumbs:crumbs});
				} else {
					self.setState({nodes:result.NodeChildren, parent:result});
				}
				
			});
		}
	},

	getInitialState: function() {
		this.getChildrenNodes();
		return {
			nodes: [],
			breadcrumbs : [{
				Id: "",
				name: "Home",
				key: "bc"
			}]
		}
	},

	handleClick: function(item) {
		this.getChildrenNodes(item);
		this.state.breadcrumbs.push({
			Id : item.Id,
			name: item.Key,
			key: item.Id + "bc"
		})
	},

	render: function(){
		var cardHide = false;
		if(!this.state.parent){
			cardHide = true;
		}

		return (
			<div>
				{
					this.state.breadcrumbs.map( crumb => {
						return <span key={crumb.key}>
							<FlatButton label={crumb.name} onClick={this.breadcrumbClick.bind(this,crumb)}/> 
							>
						</span>
					})
				}
				<Card editable={this.props.editable} cardData={this.state.parent} hide={cardHide}/>
			 	<ListNode nodes={this.state.nodes} handleClick={this.handleClick} editable={this.props.editable}/>

		 	</div>
		)
	},

	breadcrumbClick: function(crumb){ 
		var index = this.state.breadcrumbs.indexOf(crumb);
		var newCrumbs = this.state.breadcrumbs.slice(0,index+1);
		var parent = (index === 0) ? null : this.state.breadcrumbs[index];
		this.getChildrenNodes(parent,newCrumbs);
	}
});



export default ParentContainer;