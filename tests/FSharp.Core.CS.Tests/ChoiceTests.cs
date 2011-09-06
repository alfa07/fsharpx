﻿using System;
using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using NUnit.Framework;

namespace FSharp.Core.CS.Tests {
    [TestFixture]
    public class ChoiceTests {
        [Test]
        public void Match() {
            var a = FSharpChoice<int, string>.NewChoice1Of2(1);
            var c = a.Match(i => 0, _ => 1);
            Assert.AreEqual(0, c);
        }

        [Test]
        public void MatchAction() {
            var a = FSharpChoice<int, string>.NewChoice1Of2(1);
            a.Match(Console.WriteLine, _ => Assert.Fail("is string"));
        }

        [Test]
        public void New() {
            var a = FSharpChoice.New1Of2<int, string>(1);
            var b = FSharpChoice<int, string>.NewChoice1Of2(1);
            Assert.AreEqual(a, b);

            var c = FSharpChoice.New2Of2<int, string>("a");
            var d = FSharpChoice<int, string>.NewChoice2Of2("a");
            Assert.AreEqual(c, d);
        }

        [Test]
        public void Select() {
            var a = FSharpChoice.New1Of2<int, string>(5);
            var b = a.Select(i => i + 2);
            b.Match(i => Assert.AreEqual(7, i), _ => Assert.Fail("is string"));
        }

        [Test]
        public void Select2() {
            var a = FSharpChoice.New2Of2<int, string>("hello");
            var b = a.Select(i => i + 2);
            b.Match(_ => Assert.Fail("is int"), s => Assert.AreEqual("hello", s));
        }

        [Test]
        public void Validation_Errors() {
            Person.TryNew("", -5)
                .Match(_ => Assert.Fail("should not have been ok"), 
                       err => {
                           Assert.AreEqual(2, err.Length);
                           Console.WriteLine(err);
                       });
        }

        [Test]
        public void Validation_OK() {
            Person.TryNew("Pepe", 40)
                .Match(p => {
                    Assert.AreEqual("Pepe", p.Name);
                    Assert.AreEqual(40, p.Age);
                }, 
                _ => Assert.Fail("should not have failed"));
        }

    }
}