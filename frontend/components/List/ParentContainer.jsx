import React from 'react';
import ListNode from './ListNode.jsx';
import Card from '../Card.jsx';
import FlatButton from 'material-ui/FlatButton';
import Paper from 'material-ui/Paper';
import $ from 'jquery';
import config from '../../config.js';
import CircularProgress from 'material-ui/CircularProgress';
import SearchList from './SearchList.jsx';

// Component that renders the List view and Card view
var ParentContainer = React.createClass({

	// Makes requests to the database to populate card and list views.
	getChildrenNodes: function(item,crumbs){
		this.props.enableEditButton();
		this.setState({loading:true});
		// Gets the initial list of Banks
		if(!item){
			var self = this;
			$.get(config.apiHost+'items', function (result) {
				if(crumbs){
					self.setState({nodes : result, parent:null , breadcrumbs:crumbs, loading:false,search: false});
				} else {
					self.setState({nodes : result, parent:null, loading:false,search: false});
				}
			});
		// Gets data for a specfic Bank/VM
		} else {
			var self = this;
			$.get(config.apiHost+'items/' +item.Id, function (result) {
				if(crumbs){
					self.setState({nodes:result.NodeChildren, parent:result, breadcrumbs:crumbs, loading:false,search: false});
				} else {
					self.setState({nodes:result.NodeChildren, parent:result, loading:false,search: false});
				}

			});
		}
	},

	componentDidMount() {
       this.getChildrenNodes();
    },
	
	componentWillReceiveProps(next) {
		if(next.searchResult){
			var newCrumbs = this.state.breadcrumbs.slice(0,1);
			newCrumbs.push({
				name: "Search Results",
				key: "Search Results"
			});
			this.setState({breadcrumbs:newCrumbs,search:true});
		}
	},

    componentWillUnmount() {
        this.serverRequest.abort();
    },

	// Set initial states
	getInitialState: function() {
		return {
			nodes: [],
			breadcrumbs : [{
				Id: "",
				name: "Home",
				key: "bc"
			}],
			loading: true
		}
	},

	// Gets data about an item and updates the breadcrumb
	handleClick: function(item,breadcrumbFlag) {
		this.getChildrenNodes(item);

		// If breadcrumbs flag is set, update breadcrumbs
		if(!breadcrumbFlag){
			return;
		}
		this.state.breadcrumbs.push({
			Id : item.Id,
			name: item.Key,
			key: item.Id + "bc"
		});
	},

		// Gets data about an item and updates the breadcrumb
	searchResultClick: function(searchResult,breadcrumbFlag) {
		var crumbs = [{
				Id: "",
				name: "Home",
				key: "bc"
			}];
		searchResult.Path.forEach(function(element) {
			crumbs.push({
				Id : element.Key,
				name: element.Value,
				key: element.Key + "bc"
			})
		}, this);
		crumbs.pop();

		this.getChildrenNodes(searchResult, crumbs);
	},

	render: function(){
		var paperStyle = {
			width: '90%',
			margin: 'auto',
			marginTop: 10,
		};
		var center = {
			display: 'flex',
			justifyContent: 'center'
		}

		// Loading animation is shown
		if (this.state.loading){
			return (
				<Paper style= { paperStyle } zDepth= { 1}>
					<div style={center}>
						<CircularProgress size={2}/>
					</div>
				</Paper>
			)
		}

		// Search results is shown
		if(this.state.search){
			return (
			<Paper style= { paperStyle } zDepth= { 1}>
				{
					this.state.breadcrumbs.map( crumb => {
						return <span key={crumb.key}>
							<FlatButton label={crumb.name} onClick={this.breadcrumbClick.bind(this,crumb)}/>
							>
						</span>
					})
				}
				<SearchList searchResult={this.props.searchResult} searchResultClick={this.searchResultClick}/>
			</Paper >
			)
		}

		//Checks if the card needs to be hidden or not.
		var cardHide = false;
		if(!this.state.parent){
			cardHide = true;
		}

		return (
			<Paper style= { paperStyle } zDepth= { 1}>
				{
					this.state.breadcrumbs.map( crumb => {
						return <span key={crumb.key}>
							<FlatButton label={crumb.name} onClick={this.breadcrumbClick.bind(this,crumb)}/>
							>
						</span>
					})
				}
				<Card editable={this.props.editable} cardData={this.state.parent} hide={cardHide}/>
				<ListNode nodes={this.state.nodes} handleClick={this.handleClick} editable={this.props.editable} parent={this.state.parent}/>
			</Paper >
		)
		
	},

	// Handles click for breadcrumbs
	breadcrumbClick: function(crumb){
		// Remove items from breadcrumbs list
		var index = this.state.breadcrumbs.indexOf(crumb);
		var newCrumbs = this.state.breadcrumbs.slice(0,index+1);
		// Gets information about the breadcrumb clicked
		var parent = (index === 0) ? null : this.state.breadcrumbs[index];
		this.getChildrenNodes(parent,newCrumbs);
	}
});



export default ParentContainer;
