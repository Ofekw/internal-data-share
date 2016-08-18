import React from 'react';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';
import Save from 'material-ui/svg-icons/content/save';

const styles = {
  keyField: {
    width: '100px',
    marginRight: '10px',
  },
  rightAllign: {
    float: 'right',
  }
};

export default class EditModeField extends React.Component {

  constructor(props) {
    super(props);
    this.key = props.identifier || '';
    this.value = props.value || '';

  }

  handleChange(event, index, value) {
    if (value ==- 'custom') {
      // Something else
      this.setState({value});
    } else {
      this.setState({value});
    }
  }

  render() {
    return (
      <div>
        <TextField
          name="key"
          floatingLabelText="Key"
          defaultValue={this.key}
          style={styles.keyField}
        />
        <TextField
          name="value"
          floatingLabelText="Value"
          defaultValue={this.value}
          style={styles.valueField}
        />
        <RaisedButton
          icon={<Save />}
          style={styles.rightAllign}
        />
      </div>
    );
  }

}

export default EditModeField;
