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
 * Price Type
 * API Version: 2011-10-01
 * Library Version: 2016-06-01
 * Generated: Mon Jun 13 10:07:51 PDT 2016
 */


using System;
using System.Xml;
using System.Xml.Serialization;
using MWSClientCsRuntime;

namespace MarketplaceWebServiceProducts.Model
{
    [XmlTypeAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01")]
    [XmlRootAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01", IsNullable = false)]
    public class PriceType : AbstractMwsObject
    {

        private MoneyType _landedPrice;
        private MoneyType _listingPrice;
        private MoneyType _shipping;
        private Points _points;

        /// <summary>
        /// Gets and sets the LandedPrice property.
        /// </summary>
        [XmlElementAttribute(ElementName = "LandedPrice")]
        public MoneyType LandedPrice
        {
            get { return this._landedPrice; }
            set { this._landedPrice = value; }
        }

        /// <summary>
        /// Sets the LandedPrice property.
        /// </summary>
        /// <param name="landedPrice">LandedPrice property.</param>
        /// <returns>this instance.</returns>
        public PriceType WithLandedPrice(MoneyType landedPrice)
        {
            this._landedPrice = landedPrice;
            return this;
        }

        /// <summary>
        /// Checks if LandedPrice property is set.
        /// </summary>
        /// <returns>true if LandedPrice property is set.</returns>
        public bool IsSetLandedPrice()
        {
            return this._landedPrice != null;
        }

        /// <summary>
        /// Gets and sets the ListingPrice property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ListingPrice")]
        public MoneyType ListingPrice
        {
            get { return this._listingPrice; }
            set { this._listingPrice = value; }
        }

        /// <summary>
        /// Sets the ListingPrice property.
        /// </summary>
        /// <param name="listingPrice">ListingPrice property.</param>
        /// <returns>this instance.</returns>
        public PriceType WithListingPrice(MoneyType listingPrice)
        {
            this._listingPrice = listingPrice;
            return this;
        }

        /// <summary>
        /// Checks if ListingPrice property is set.
        /// </summary>
        /// <returns>true if ListingPrice property is set.</returns>
        public bool IsSetListingPrice()
        {
            return this._listingPrice != null;
        }

        /// <summary>
        /// Gets and sets the Shipping property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Shipping")]
        public MoneyType Shipping
        {
            get { return this._shipping; }
            set { this._shipping = value; }
        }

        /// <summary>
        /// Sets the Shipping property.
        /// </summary>
        /// <param name="shipping">Shipping property.</param>
        /// <returns>this instance.</returns>
        public PriceType WithShipping(MoneyType shipping)
        {
            this._shipping = shipping;
            return this;
        }

        /// <summary>
        /// Checks if Shipping property is set.
        /// </summary>
        /// <returns>true if Shipping property is set.</returns>
        public bool IsSetShipping()
        {
            return this._shipping != null;
        }

        /// <summary>
        /// Gets and sets the Points property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Points")]
        public Points Points
        {
            get { return this._points; }
            set { this._points = value; }
        }

        /// <summary>
        /// Sets the Points property.
        /// </summary>
        /// <param name="points">Points property.</param>
        /// <returns>this instance.</returns>
        public PriceType WithPoints(Points points)
        {
            this._points = points;
            return this;
        }

        /// <summary>
        /// Checks if Points property is set.
        /// </summary>
        /// <returns>true if Points property is set.</returns>
        public bool IsSetPoints()
        {
            return this._points != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _landedPrice = reader.Read<MoneyType>("LandedPrice");
            _listingPrice = reader.Read<MoneyType>("ListingPrice");
            _shipping = reader.Read<MoneyType>("Shipping");
            _points = reader.Read<Points>("Points");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("LandedPrice", _landedPrice);
            writer.Write("ListingPrice", _listingPrice);
            writer.Write("Shipping", _shipping);
            writer.Write("Points", _points);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonservices.com/schema/Products/2011-10-01", "PriceType", this);
        }

        public PriceType() : base()
        {
        }
    }
}
