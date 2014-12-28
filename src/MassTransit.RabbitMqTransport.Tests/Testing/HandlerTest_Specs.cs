// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.RabbitMqTransport.Tests.Testing
{
    using System.Linq;
    using NUnit.Framework;
    using MassTransit.Testing;
    using Shouldly;


    [Ignore("This is broken because RabbitMQ does not have subscriptions")]
	public class Using_the_handler_test_factory
	{
		IHandlerTest<A> _test;

		[SetUp]
		public void Setup()
		{
			_test = TestFactory.ForHandler<A>()
				.New(x =>
					{
//						x.UseRabbitMqBusScenario();

						x.Send(new A());
						x.Send(new B());
					});
			_test.Execute();
		}

		[TearDown]
		public void Teardown()
		{
			_test.Dispose();
			_test = null;
		}

		[Test]
		public void Should_have_received_a_message_of_type_a()
		{
            _test.Received.Select<A>().Any().ShouldBe(true);
		}

		[Test]
		public void Should_have_skipped_a_message_of_type_b()
		{
            _test.Skipped.Select<B>().Any().ShouldBe(true);
		}

		[Test]
		public void Should_not_have_skipped_a_message_of_type_a()
		{
            _test.Skipped.Select<A>().Any().ShouldBe(false);
		}

		[Test]
		public void Should_have_sent_a_message_of_type_a()
		{
            _test.Sent.Select<A>().Any().ShouldBe(true);
		}

		[Test]
		public void Should_have_sent_a_message_of_type_b()
		{
            _test.Sent.Select<B>().Any().ShouldBe(true);
		}

		[Test]
		public void Should_support_a_simple_handler()
		{
            _test.Handler.Received.Select().Any().ShouldBe(true);
		}

		class A
		{
		}

		class B
		{
		}
	}
}