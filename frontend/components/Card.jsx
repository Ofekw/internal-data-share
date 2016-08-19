import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import NormalModeField from './NormalModeField.jsx';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
};


class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);

    this.children = [
      <NormalModeField key="1" identifier="Url" value="http://ec1-257-0-123-999.ap-northnortheast-2.compute.amazonaws.com"/>,
      <NormalModeField key="2" editable="true" identifier="Password" value="hunter2"/>,
      <NormalModeField key="3" identifier="Some other key" value="Hello World"/>,
    ];

    this.state = {
      open: false
    };
  }

  createNew = () => {
    this.children.push(<NormalModeField editable="true" key={Date.now()}/>);
    this.forceUpdate();
  }

  render() {
    return (
      <Card>
        <CardHeader
          title="ABC Bank VM_1"
          subtitle="Subtitle?"
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
