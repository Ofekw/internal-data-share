import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import Dialog from 'material-ui/Dialog';
import TextField from 'material-ui/TextField';
import ModalField from './ModalField.jsx';

// Card for displaying information for an environemnt.
class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
    this.labels = [];
    this.title = '';
    this.id = -1;
  }

  state = {
    open: false,
  };

  handleOpen = () => {
    this.setState({open: true});
  };

  handleClose = () => {
    this.setState({open: false});
  };

  handleLabelSave = () => {
    this.setState({open: false});
    this.createNewLabel();
  };

  // Add a new child.
  createNewField = () => {
    this.props.cardData.LeafChildren.push({
      'Key': '',
      'Value': '',
      'new': true
    });
    this.forceUpdate();
  };

  // Add a new child.
  createNewLabel = () => {
    if (this.nextLabel !== '') {
      this.props.cardData.Labels.push(this.nextLabel);
      this.nextLabel = '';
    }
    this.forceUpdate();
  };

  handleLabelChange = (event) => {
    this.nextLabel = event.target.value;
  }

  render() {
    if (this.props.hide) {
      return <div></div>
    }

    if(this.props.cardData) {
      this.title = this.props.cardData.Key;
      const leafChildren = this.props.cardData.LeafChildren;

      this.children = [];

      for (var child in leafChildren) {
        // Add all the children.
        if (leafChildren.hasOwnProperty(child)) {
          const childElement = leafChildren[child];
          this.children.push(
            <ModalField new={childElement.new} editable={this.props.editable} key={childElement.Id} childId={childElement.Id} identifier={childElement.Key} value={childElement.Value} parentId={this.props.cardData.Id} />
          );
        }
      }
    }

    var itemStyle = {
			width: '100%',
			display: 'inline-block',
			position: 'relative'
		};

		var buttonStyle = {
			display: 'inline-block',
			position: 'relative',
		};

    const actions = [
      <FlatButton
        label="Cancel"
        primary={true}
        onTouchTap={this.handleClose}
      />,
      <FlatButton
        label="Submit"
        primary={true}
        keyboardFocused={true}
        onTouchTap={this.handleLabelSave}
      />,
    ];

    return (
      <Card>
        <CardHeader
          title={this.title}
          actAsExpander={false}
          showExpandableButton={false}
          />
        <List ref="theList">
          {this.children.map(function (child, index) {
            // Add all the children.
            return child;
          }) }
        </List>
        <CardActions>
          {(() => {
            // Immediately invoked function to add "New" button if in editable mode.
            if (this.props.editable) {
              return <div>
                <FlatButton style={buttonStyle} label="Add Key Pair" secondary={true}  onTouchTap={this.createNewField}/>
                <FlatButton style={buttonStyle} label="Add Label" secondary={true}  onTouchTap={this.handleOpen}/>
                <Dialog
                  title="Dialog With Actions"
                  actions={actions}
                  modal={false}
                  open={this.state.open}
                  onRequestClose={this.handleClose}
                >
                  <TextField
                    floatingLabelText="Label Text"
                    onChange={this.handleLabelChange}
                  />
                </Dialog>
              </div>
            }
          })() }
        </CardActions>
      </Card>

    );
  }
}

export default CardExampleExpandable;
