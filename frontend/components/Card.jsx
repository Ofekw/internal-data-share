import React from 'react';
import $ from 'jquery';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import NormalModeField from './NormalModeField.jsx';

class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
    this.title = '';
  }

  createNew = () => {
    this.children.push(<NormalModeField editable="true" key={Date.now()}/>);
    this.forceUpdate();
  }

  render() {
    if(this.props.cardData) {
      this.title = this.props.cardData.Key;
      const leafChildren = this.props.cardData.LeafChildren;
      for (var child in leafChildren) {
        if (leafChildren.hasOwnProperty(child)) {
          const childElement = leafChildren[child];
          this.children.push(
            <NormalModeField key={childElement.Id} identifier={childElement.Key} value={childElement.Value} />
          );
        }
      }
    }

    return (
      <Card>
        <CardHeader
          title={this.title}
          actAsExpander={false}
          showExpandableButton={false}
        />
        <List ref="theList">
          {this.children.map(function(child, index) {
            return child;
          })}
        </List>
        <CardActions>
          <FlatButton label="New" onTouchTap={this.createNew}/>
        </CardActions>
      </Card>
    );
  }
}
export default CardExampleExpandable;
