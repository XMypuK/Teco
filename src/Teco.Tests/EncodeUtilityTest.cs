#region License
// Copyright (c) 2019 Zeiler Evgenii, https://github.com/XMypuK
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using NUnit.Framework;
using System;
using Teco.Utilities;

namespace Teco.Tests {
    class EncodeUtilityTest {
        [Test]
        public void ENC_UrlEncode() {
            String sourceSet = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0a\x0b\x0c\x0d\x0e\x0f"
                + "\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f"
                + "\x20\x21\x22\x23\x24\x25\x26\x27\x28\x29\x2a\x2b\x2c\x2d\x2e\x2f"
                + "\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39\x3a\x3b\x3c\x3d\x3e\x3f"
                + "\x40\x41\x42\x43\x44\x45\x46\x47\x48\x49\x4a\x4b\x4c\x4d\x4e\x4f"
                + "\x50\x51\x52\x53\x54\x55\x56\x57\x58\x59\x5a\x5b\x5c\x5d\x5e\x5f"
                + "\x60\x61\x62\x63\x64\x65\x66\x67\x68\x69\x6a\x6b\x6c\x6d\x6e\x6f"
                + "\x70\x71\x72\x73\x74\x75\x76\x77\x78\x79\x7a\x7b\x7c\x7d\x7e\x7f"
                + "АБВГДЕЁЖЗИЙКЛМНО"
                + "ПРСТУФХЦЧШЩъЫьЭЮ"
                + "Яабвгдеёжзийклмн"
                + "опрстуфхцчшщъыьэ"
                + "юя";

            String resultAsUrlWithoutUnicodeEncoding = "%00%01%02%03%04%05%06%07%08%09%0A%0B%0C%0D%0E%0F"
                + "%10%11%12%13%14%15%16%17%18%19%1A%1B%1C%1D%1E%1F"
                + "%20!%22#$%&'()*+,-./"
                + "0123456789:;%3C=%3E?"
                + "@ABCDEFGHIJKLMNO"
                + "PQRSTUVWXYZ[%5C]%5E_"
                + "%60abcdefghijklmno"
                + "pqrstuvwxyz%7B%7C%7D~%7F"
                + "АБВГДЕЁЖЗИЙКЛМНО"
                + "ПРСТУФХЦЧШЩъЫьЭЮ"
                + "Яабвгдеёжзийклмн"
                + "опрстуфхцчшщъыьэ"
                + "юя";

            String resultAsUrlWithUnicodeEncoding = "%00%01%02%03%04%05%06%07%08%09%0A%0B%0C%0D%0E%0F"
                + "%10%11%12%13%14%15%16%17%18%19%1A%1B%1C%1D%1E%1F"
                + "%20!%22#$%&'()*+,-./"
                + "0123456789:;%3C=%3E?"
                + "@ABCDEFGHIJKLMNO"
                + "PQRSTUVWXYZ[%5C]%5E_"
                + "%60abcdefghijklmno"
                + "pqrstuvwxyz%7B%7C%7D~%7F"
                + "%D0%90%D0%91%D0%92%D0%93%D0%94%D0%95%D0%81%D0%96%D0%97%D0%98%D0%99%D0%9A%D0%9B%D0%9C%D0%9D%D0%9E"
                + "%D0%9F%D0%A0%D0%A1%D0%A2%D0%A3%D0%A4%D0%A5%D0%A6%D0%A7%D0%A8%D0%A9%D1%8A%D0%AB%D1%8C%D0%AD%D0%AE"
                + "%D0%AF%D0%B0%D0%B1%D0%B2%D0%B3%D0%B4%D0%B5%D1%91%D0%B6%D0%B7%D0%B8%D0%B9%D0%BA%D0%BB%D0%BC%D0%BD"
                + "%D0%BE%D0%BF%D1%80%D1%81%D1%82%D1%83%D1%84%D1%85%D1%86%D1%87%D1%88%D1%89%D1%8A%D1%8B%D1%8C%D1%8D"
                + "%D1%8E%D1%8F";

            String resultAsSegmentWithoutUnicodeEncoding = "%00%01%02%03%04%05%06%07%08%09%0A%0B%0C%0D%0E%0F"
                + "%10%11%12%13%14%15%16%17%18%19%1A%1B%1C%1D%1E%1F"
                + "%20%21%22%23%24%25%26%27%28%29%2A%2B%2C-.%2F"
                + "0123456789%3A%3B%3C%3D%3E%3F"
                + "%40ABCDEFGHIJKLMNO"
                + "PQRSTUVWXYZ%5B%5C%5D%5E_"
                + "%60abcdefghijklmno"
                + "pqrstuvwxyz%7B%7C%7D~%7F"
                + "АБВГДЕЁЖЗИЙКЛМНО"
                + "ПРСТУФХЦЧШЩъЫьЭЮ"
                + "Яабвгдеёжзийклмн"
                + "опрстуфхцчшщъыьэ"
                + "юя";

            String resultAsSegmentWithUnicodeEncoding = "%00%01%02%03%04%05%06%07%08%09%0A%0B%0C%0D%0E%0F"
                + "%10%11%12%13%14%15%16%17%18%19%1A%1B%1C%1D%1E%1F"
                + "%20%21%22%23%24%25%26%27%28%29%2A%2B%2C-.%2F"
                + "0123456789%3A%3B%3C%3D%3E%3F"
                + "%40ABCDEFGHIJKLMNO"
                + "PQRSTUVWXYZ%5B%5C%5D%5E_"
                + "%60abcdefghijklmno"
                + "pqrstuvwxyz%7B%7C%7D~%7F"
                + "%D0%90%D0%91%D0%92%D0%93%D0%94%D0%95%D0%81%D0%96%D0%97%D0%98%D0%99%D0%9A%D0%9B%D0%9C%D0%9D%D0%9E"
                + "%D0%9F%D0%A0%D0%A1%D0%A2%D0%A3%D0%A4%D0%A5%D0%A6%D0%A7%D0%A8%D0%A9%D1%8A%D0%AB%D1%8C%D0%AD%D0%AE"
                + "%D0%AF%D0%B0%D0%B1%D0%B2%D0%B3%D0%B4%D0%B5%D1%91%D0%B6%D0%B7%D0%B8%D0%B9%D0%BA%D0%BB%D0%BC%D0%BD"
                + "%D0%BE%D0%BF%D1%80%D1%81%D1%82%D1%83%D1%84%D1%85%D1%86%D1%87%D1%88%D1%89%D1%8A%D1%8B%D1%8C%D1%8D"
                + "%D1%8E%D1%8F";

            Assert.IsNull(EncodeUtility.UrlEncode(null, false));
            Assert.AreEqual("", EncodeUtility.UrlEncode("", false));
            Assert.AreEqual(resultAsSegmentWithoutUnicodeEncoding, EncodeUtility.UrlEncode(sourceSet, true));
            Assert.AreEqual(resultAsSegmentWithUnicodeEncoding, EncodeUtility.UrlEncode(sourceSet, false));

            Assert.IsNull(EncodeUtility.UrlEncodeAsUrl(null, false));
            Assert.AreEqual("", EncodeUtility.UrlEncodeAsUrl("", false));
            Assert.AreEqual(resultAsUrlWithoutUnicodeEncoding, EncodeUtility.UrlEncodeAsUrl(sourceSet, true));
            Assert.AreEqual(resultAsUrlWithUnicodeEncoding, EncodeUtility.UrlEncodeAsUrl(sourceSet, false));
        }

        [Test]
        public void ENC_HtmlEncode() {
            String sourceSet = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0a\x0b\x0c\x0d\x0e\x0f"
                + "\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f"
                + "\x20\x21\x22\x23\x24\x25\x26\x27\x28\x29\x2a\x2b\x2c\x2d\x2e\x2f"
                + "\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39\x3a\x3b\x3c\x3d\x3e\x3f"
                + "\x40\x41\x42\x43\x44\x45\x46\x47\x48\x49\x4a\x4b\x4c\x4d\x4e\x4f"
                + "\x50\x51\x52\x53\x54\x55\x56\x57\x58\x59\x5a\x5b\x5c\x5d\x5e\x5f"
                + "\x60\x61\x62\x63\x64\x65\x66\x67\x68\x69\x6a\x6b\x6c\x6d\x6e\x6f"
                + "\x70\x71\x72\x73\x74\x75\x76\x77\x78\x79\x7a\x7b\x7c\x7d\x7e\x7f"
                + "\x80\x81\x82\x83\x84\x85\x86\x87\x88\x89\x8a\x8b\x8c\x8d\x8e\x8f"
                + "\x90\x91\x92\x93\x94\x95\x96\x97\x98\x99\x9a\x9b\x9c\x9d\x9e\x9f"
                + "\xa0\xa1\xa2\xa3\xa4\xa5\xa6\xa7\xa8\xa9\xaa\xab\xac\xad\xae\xaf"
                + "\xb0\xb1\xb2\xb3\xb4\xb5\xb6\xb7\xb8\xb9\xba\xbb\xbc\xbd\xbe\xbf"
                + "\xc0\xc1\xc2\xc3\xc4\xc5\xc6\xc7\xc8\xc9\xca\xcb\xcc\xcd\xce\xcf"
                + "\xd0\xd1\xd2\xd3\xd4\xd5\xd6\xd7\xd8\xd9\xda\xdb\xdc\xdd\xde\xdf"
                + "\xe0\xe1\xe2\xe3\xe4\xe5\xe6\xe7\xe8\xe9\xea\xeb\xec\xed\xee\xef"
                + "\xf0\xf1\xf2\xf3\xf4\xf5\xf6\xf7\xf8\xf9\xfa\xfb\xfc\xfd\xfe\xff"
                + "АБВГДЕЁЖЗИЙКЛМНО"
                + "ПРСТУФХЦЧШЩъЫьЭЮ"
                + "Яабвгдеёжзийклмн"
                + "опрстуфхцчшщъыьэ"
                + "юя";

            String result = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0a\x0b\x0c\x0d\x0e\x0f"
                + "\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f"
                + "\x20\x21&quot;\x23\x24\x25&amp;\x27\x28\x29\x2a\x2b\x2c\x2d\x2e\x2f"
                + "\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39\x3a\x3b&lt;\x3d&gt;\x3f"
                + "\x40\x41\x42\x43\x44\x45\x46\x47\x48\x49\x4a\x4b\x4c\x4d\x4e\x4f"
                + "\x50\x51\x52\x53\x54\x55\x56\x57\x58\x59\x5a\x5b\x5c\x5d\x5e\x5f"
                + "\x60\x61\x62\x63\x64\x65\x66\x67\x68\x69\x6a\x6b\x6c\x6d\x6e\x6f"
                + "\x70\x71\x72\x73\x74\x75\x76\x77\x78\x79\x7a\x7b\x7c\x7d\x7e\x7f"
                + "\x80\x81\x82\x83\x84\x85\x86\x87\x88\x89\x8a\x8b\x8c\x8d\x8e\x8f"
                + "\x90\x91\x92\x93\x94\x95\x96\x97\x98\x99\x9a\x9b\x9c\x9d\x9e\x9f"
                + "&#160;&#161;&#162;&#163;&#164;&#165;&#166;&#167;&#168;&#169;&#170;&#171;&#172;&#173;&#174;&#175;"
                + "&#176;&#177;&#178;&#179;&#180;&#181;&#182;&#183;&#184;&#185;&#186;&#187;&#188;&#189;&#190;&#191;"
                + "&#192;&#193;&#194;&#195;&#196;&#197;&#198;&#199;&#200;&#201;&#202;&#203;&#204;&#205;&#206;&#207;"
                + "&#208;&#209;&#210;&#211;&#212;&#213;&#214;&#215;&#216;&#217;&#218;&#219;&#220;&#221;&#222;&#223;"
                + "&#224;&#225;&#226;&#227;&#228;&#229;&#230;&#231;&#232;&#233;&#234;&#235;&#236;&#237;&#238;&#239;"
                + "&#240;&#241;&#242;&#243;&#244;&#245;&#246;&#247;&#248;&#249;&#250;&#251;&#252;&#253;&#254;&#255;"
                + "АБВГДЕЁЖЗИЙКЛМНО"
                + "ПРСТУФХЦЧШЩъЫьЭЮ"
                + "Яабвгдеёжзийклмн"
                + "опрстуфхцчшщъыьэ"
                + "юя";

            Assert.IsNull(EncodeUtility.HtmlEncode(null));
            Assert.AreEqual("", EncodeUtility.HtmlEncode(""));
            Assert.AreEqual(result, EncodeUtility.HtmlEncode(sourceSet));
        }
    }
}
