using Azure.Messaging.ServiceBus;
using DavidBerry.Framework.Messaging;
using Shouldly;
using System.Reflection;
using Xunit;

namespace DavidBerry.Framework.Messaging.AzServiceBus.Tests
{
    public class AzServiceBusMessagePublisherTests
    {
        /// <summary>
        /// Tests that ApplyPropertiesToMessage applies the MessageId property when provided.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithMessageId_ShouldSetMessageId()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var messageId = "test-message-id-123";
            var properties = MessageProperties.Create(messageId: messageId);

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.MessageId.ShouldBe(messageId);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage applies the Subject property when provided.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithSubject_ShouldSetSubject()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var subject = "test-subject";
            var properties = MessageProperties.Create(subject: subject);

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.Subject.ShouldBe(subject);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage applies the CorrelationId property when provided.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithCorrelationId_ShouldSetCorrelationId()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var correlationId = "test-correlation-id-456";
            var properties = MessageProperties.Create(correlationId: correlationId);

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.CorrelationId.ShouldBe(correlationId);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage applies ApplicationProperties when provided.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithApplicationProperties_ShouldSetApplicationProperties()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var appProperties = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 42 },
                { "key3", true }
            };
            var properties = MessageProperties.Create(applicationProperties: appProperties);

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.ApplicationProperties.ShouldContainKeyAndValue("key1", "value1");
            serviceBusMessage.ApplicationProperties.ShouldContainKeyAndValue("key2", 42);
            serviceBusMessage.ApplicationProperties.ShouldContainKeyAndValue("key3", true);
            serviceBusMessage.ApplicationProperties.Count.ShouldBe(3);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage applies all properties together.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithAllProperties_ShouldSetAllProperties()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var messageId = "msg-id-789";
            var subject = "important-subject";
            var correlationId = "corr-id-101112";
            var appProperties = new Dictionary<string, object>
            {
                { "department", "sales" },
                { "priority", 1 }
            };
            var properties = MessageProperties.Create(
                messageId: messageId,
                subject: subject,
                correlationId: correlationId,
                applicationProperties: appProperties
            );

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.MessageId.ShouldBe(messageId);
            serviceBusMessage.Subject.ShouldBe(subject);
            serviceBusMessage.CorrelationId.ShouldBe(correlationId);
            serviceBusMessage.ApplicationProperties.ShouldContainKeyAndValue("department", "sales");
            serviceBusMessage.ApplicationProperties.ShouldContainKeyAndValue("priority", 1);
            serviceBusMessage.ApplicationProperties.Count.ShouldBe(2);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage does nothing when properties is null.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithNullProperties_ShouldNotThrow()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");

            // Act & Assert
            var exception = Record.Exception(() => InvokeApplyPropertiesToMessage(serviceBusMessage, null));
            exception.ShouldBeNull();
            serviceBusMessage.MessageId.ShouldBeNull();
            serviceBusMessage.Subject.ShouldBeNull();
            serviceBusMessage.CorrelationId.ShouldBeNull();
            serviceBusMessage.ApplicationProperties.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage ignores empty MessageId.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithEmptyMessageId_ShouldNotSet()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var properties = MessageProperties.Create(messageId: "");

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.MessageId.ShouldBeNull();
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage ignores whitespace-only MessageId.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithWhitespaceMessageId_ShouldNotSet()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var properties = MessageProperties.Create(messageId: "   ");

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.MessageId.ShouldBeNull();
        }

        /// <summary>
        /// Tests that ApplyPropertiesToMessage ignores empty ApplicationProperties.
        /// </summary>
        [Fact]
        public void ApplyPropertiesToMessage_WithEmptyApplicationProperties_ShouldNotAddAny()
        {
            // Arrange
            var serviceBusMessage = new ServiceBusMessage("test body");
            var emptyAppProperties = new Dictionary<string, object>();
            var properties = MessageProperties.Create(applicationProperties: emptyAppProperties);

            // Act
            InvokeApplyPropertiesToMessage(serviceBusMessage, properties);

            // Assert
            serviceBusMessage.ApplicationProperties.Count.ShouldBe(0);
        }

        /// <summary>
        /// Helper method to invoke the private ApplyPropertiesToMessage method using reflection.
        /// </summary>
        private static void InvokeApplyPropertiesToMessage(ServiceBusMessage serviceBusMessage, MessageProperties? properties)
        {
            var method = typeof(AzServiceBusMessagePublisher<>)
                .MakeGenericType(typeof(object))
                .GetMethod("ApplyPropertiesToMessage", 
                    BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    new[] { typeof(ServiceBusMessage), typeof(MessageProperties) },
                    null);

            method.ShouldNotBeNull("ApplyPropertiesToMessage method should exist");
            method!.Invoke(null, new object?[] { serviceBusMessage, properties });
        }
    }
}
