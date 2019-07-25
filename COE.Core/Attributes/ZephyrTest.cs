using System;

namespace COE.Core.Attributes
{
    /// <summary>
    /// Specifies the Zephyr Test ID related to the test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ZephyrTestAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ZephyrTestAttribute"/> class.</summary>
        /// <param name="value">The value of attribute.</param>
        public ZephyrTestAttribute(string value)
        {
            //@todo: relpace thet for using https://[Config.JIRA.REST.API.URL]/rest/api/2/issue/[value]
            //to allow use Issue key instead of issue.id and get id from jira rest api
            //add [Config.JIRA.REST.API.URL] variable into config for this
            Value = value;
        }

        /// <summary>
        /// Gets the ZephyrTest value.
        /// </summary>
        public string Value { get; private set; }
    }
}
