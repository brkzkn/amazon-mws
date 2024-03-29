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
 * ASIN Offer Detail List
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
    public class ASINOfferDetailList : AbstractMwsObject
    {

        private List<ASINOfferDetail> _offer;

        /// <summary>
        /// Gets and sets the Offer property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Offer")]
        public List<ASINOfferDetail> Offer
        {
            get
            {
                if(this._offer == null)
                {
                    this._offer = new List<ASINOfferDetail>();
                }
                return this._offer;
            }
            set { this._offer = value; }
        }

        /// <summary>
        /// Sets the Offer property.
        /// </summary>
        /// <param name="offer">Offer property.</param>
        /// <returns>this instance.</returns>
        public ASINOfferDetailList WithOffer(ASINOfferDetail[] offer)
        {
            this._offer.AddRange(offer);
            return this;
        }

        /// <summary>
        /// Checks if Offer property is set.
        /// </summary>
        /// <returns>true if Offer property is set.</returns>
        public bool IsSetOffer()
        {
            return this.Offer.Count > 0;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _offer = reader.ReadList<ASINOfferDetail>("Offer");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.WriteList("Offer", _offer);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonservices.com/schema/Products/2011-10-01", "ASINOfferDetailList", this);
        }

        public ASINOfferDetailList() : base()
        {
        }
    }
}
