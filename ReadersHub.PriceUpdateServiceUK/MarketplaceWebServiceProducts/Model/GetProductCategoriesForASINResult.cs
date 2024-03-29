/*******************************************************************************
 * Copyright 2009-2016 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Get Product Categories For ASIN Result
 * API Version: 2011-10-01
 * Library Version: 2016-06-01
 * Generated: Mon Jun 13 10:07:51 PDT 2016
 */


using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using MWSClientCsRuntime;

namespace MarketplaceWebServiceProducts.Model
{
    [XmlTypeAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01")]
    [XmlRootAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01", IsNullable = false)]
    public class GetProductCategoriesForASINResult : AbstractMwsObject
    {

        private List<Categories> _self;

        /// <summary>
        /// Gets and sets the Self property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Self")]
        public List<Categories> Self
        {
            get
            {
                if(this._self == null)
                {
                    this._self = new List<Categories>();
                }
                return this._self;
            }
            set { this._self = value; }
        }

        /// <summary>
        /// Sets the Self property.
        /// </summary>
        /// <param name="self">Self property.</param>
        /// <returns>this instance.</returns>
        public GetProductCategoriesForASINResult WithSelf(Categories[] self)
        {
            this._self.AddRange(self);
            return this;
        }

        /// <summary>
        /// Checks if Self property is set.
        /// </summary>
        /// <returns>true if Self property is set.</returns>
        public bool IsSetSelf()
        {
            return this.Self.Count > 0;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _self = reader.ReadList<Categories>("Self");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.WriteList("Self", _self);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonservices.com/schema/Products/2011-10-01", "GetProductCategoriesForASINResult", this);
        }

        public GetProductCategoriesForASINResult() : base()
        {
        }
    }
}
