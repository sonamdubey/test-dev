import React from "react";
import ThankYouHeader from "../../../src/LeadForm/Components/ThankYouHeader";

test("Component ThankYouHeader Snapshot RenderedTree", () => {
  const props = {
    name: "test"
  };

  let wrapper = shallow(<ThankYouHeader {...props} />);
  expect(wrapper).toMatchSnapshot();
});
