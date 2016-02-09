// __tests__/CheckboxWithLabel-test.js
jest.dontMock('../Game/CannotPlay.jsx');

import React from 'react';
import ReactDOM from 'react-dom';
import TestUtils from 'react-addons-test-utils';

const CannotPlay = require('../Game/CannotPlay.jsx').default;

describe('CheckboxWithLabel', () => {

  it('changes the text after click', () => {
      var myMock = jest.genMockFunction();
    // Render a checkbox with label in the document
    var checkbox = TestUtils.renderIntoDocument(
      <CannotPlay cannotPlayCards={myMock} disabled={false} />
    );

    var checkboxNode = ReactDOM.findDOMNode(checkbox);

    // Verify that it's Off by default
    expect(checkboxNode.textContent).toEqual('I can not play a card!');

    TestUtils.Simulate.click(checkboxNode);
    expect(myMock).toBeCalled();
  });

});