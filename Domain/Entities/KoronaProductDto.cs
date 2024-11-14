namespace Domain.Entities
{
  using System;
  using System.Collections.Generic;

  public class KoronaProductDto
  {
    public bool Active { get; set; }
    public string Id { get; set; }
    public int Revision { get; set; }
    public string Number { get; set; }
    public Sector AlternativeSector { get; set; }
    public Assortment Assortment { get; set; }
    public List<Code> Codes { get; set; }
    public CommodityGroup CommodityGroup { get; set; }
    public bool Conversion { get; set; }
    public decimal Costs { get; set; }
    public bool Deactivated { get; set; }
    public bool Deposit { get; set; }
    public List<Description> Descriptions { get; set; }
    public bool Discountable { get; set; }
    public Image Image { get; set; }
    public List<InfoText> InfoTexts { get; set; }
    public ItemSequence ItemSequence { get; set; }
    public decimal LastPurchasePrice { get; set; }
    public bool Listed { get; set; }
    public DateTime ListedSince { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public string Name { get; set; }
    public int PackagingQuantity { get; set; }
    public bool PackagingRequired { get; set; }
    public string PackagingUnit { get; set; }
    public bool PersonalizationRequired { get; set; }
    public bool PriceChangable { get; set; }
    public List<Price> Prices { get; set; }
    public bool PrintTicketsSeparately { get; set; }
    public ProductionType ProductionType { get; set; }
    public decimal RecommendedRetailPrice { get; set; }
    public RelatedProducts RelatedProducts { get; set; }
    public Sector Sector { get; set; }
    public bool SerialNumberRequired { get; set; }
    public string SubproductPresentation { get; set; }
    public List<Subproduct> Subproducts { get; set; }
    public List<SupplierPrice> SupplierPrices { get; set; }
    public List<Tag> Tags { get; set; }
    public TicketDefinition TicketDefinition { get; set; }
    public bool TrackInventory { get; set; }
    public List<MediaUrl> MediaUrls { get; set; }
    public int QuantityDenomination { get; set; }
    public List<SpecialPrice> SpecialPrices { get; set; }
    public List<VerificationRequirement> VerificationRequirements { get; set; }
    public bool SalesLock { get; set; }
    public Dictionary<string, string> CustomProperties { get; set; }
    public List<Container> Containers { get; set; }
    public int ContainerCapacity { get; set; }
    public bool IndependentSubarticleDiscounts { get; set; }
    public List<ListedOrganizationalUnit> ListedOrganizationalUnits { get; set; }
    public bool StockReturnUnsellable { get; set; }
  }

  public class Sector
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Assortment
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Code
  {
    public string ProductCode { get; set; }
    public int ContainerSize { get; set; }
    public string Description { get; set; }
  }

  public class CommodityGroup
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Description
  {
    public string Type { get; set; }
    public string Text { get; set; }
    public Language Language { get; set; }
  }

  public class Language
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Image
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class InfoText
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class ItemSequence
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Price
  {
    public decimal Value { get; set; }
    public DateTime ValidFrom { get; set; }
    public string ProductCode { get; set; }
    public PriceGroup PriceGroup { get; set; }
    public OrganizationalUnit OrganizationalUnit { get; set; }
    public string Number { get; set; }
  }

  public class PriceGroup
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class OrganizationalUnit
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class ProductionType
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class RelatedProducts
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Subproduct
  {
    public List<Price> Prices { get; set; }
    public ProductReference Product { get; set; }
    public int Quantity { get; set; }
    public Tag Tag { get; set; }
    public string Type { get; set; }
  }

  public class ProductReference
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class SupplierPrice
  {
    public Supplier Supplier { get; set; }
    public string OrderCode { get; set; }
    public decimal Value { get; set; }
    public int ContainerSize { get; set; }
    public string Description { get; set; }
  }

  public class Supplier
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Tag
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class TicketDefinition
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class MediaUrl
  {
    public string Url { get; set; }
  }

  public class SpecialPrice
  {
    public decimal Value { get; set; }
    public int QuantityFrom { get; set; }
    public SpecialPriceConfiguration SpecialPriceConfiguration { get; set; }
  }

  public class SpecialPriceConfiguration
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class VerificationRequirement
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

  public class Container
  {
    public List<Price> Prices { get; set; }
    public ProductReference Product { get; set; }
    public string Description { get; set; }
    public bool DefaultContainer { get; set; }
  }

  public class ListedOrganizationalUnit
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
  }

}
