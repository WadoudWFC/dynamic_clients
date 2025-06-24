using MultipleHttpClient.Application.Interfaces.Reference;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class ReferenceAglouDataService : IReferenceAglouDataService
{
    private readonly IHttpReferenceAglouDataService _clientService;
    private readonly IReferenceDataMappingService _referenceDataMappingService;
    public ReferenceAglouDataService(IHttpReferenceAglouDataService httpReferenceAglouDataService, IReferenceDataMappingService referenceDataMappingService)
    {
        _clientService = httpReferenceAglouDataService;
        _referenceDataMappingService = referenceDataMappingService;
    }

    public async Task<Result<IEnumerable<ActivityNatureSanitized>>> GetAllActivitiesAsync(GetAllActivitiesQuery query)
    {
        var response = await _clientService.GetAllActivitiesAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<ActivityNatureSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Actitivites"));
        }
        var activities = response.Data.Data.Select(activity => new ActivityNatureSanitized(Id: _referenceDataMappingService.GetOrCreateGuidForReferenceId(activity.Id, Constants.Activity),
                                                             ActivityNature: activity.ActivityNature, ActivityName: activity.ActivityName)
        { InternalId = activity.Id });
        return Result<IEnumerable<ActivityNatureSanitized>>.Success(activities);
    }

    public async Task<Result<IEnumerable<CitiesSanitized>>> GetAllCitiesAsync(GetAllCitiesQuery query)
    {
        var response = await _clientService.GetAllCitiesAsync();
        if (!response.IsSuccess || response.Data?.Data == null) return Result<IEnumerable<CitiesSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve cities"));

        // Pre-fetch all region and decoupage commercial mappings for batch conversion
        var cityData = response.Data?.Data.ToList();
        if (cityData == null) return Result<IEnumerable<CitiesSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve cities"));

        var regionIds = cityData.Select(c => c.Id_Region).Where(id => id.HasValue).Distinct();
        var decoupageCommercialIds = cityData.Select(c => c.Id_DecoupageCommercial).Where(id => id.HasValue).Distinct();

        var regionMappings = regionIds.ToDictionary(id => id.Value, id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id.Value, Constants.Region));
        var decoupageMappings = decoupageCommercialIds.ToDictionary(id => id.Value, id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id.Value, Constants.Decoupage));

        var cities = cityData.Select(city => new CitiesSanitized(City: _referenceDataMappingService.GetOrCreateGuidForReferenceId(city.Id, Constants.City),
                                                Label: city.Label,
                                                Region: city.Id_Region.HasValue ? regionMappings[city.Id_Region.Value] : null,
                                                DecoupageCommercial: city.Id_DecoupageCommercial.HasValue ? decoupageMappings[city.Id_DecoupageCommercial.Value] : null,
                                                LocalityType: city.Type_Localite,
                                                Logo: city.Logo)
        {
            CityId = city.Id,
            RegionId = city.Id_Region,
            DecoupageCommercialId = city.Id_DecoupageCommercial
        });
        return Result<IEnumerable<CitiesSanitized>>.Success(cities);
    }

    public async Task<Result<IEnumerable<RegionsSanitized>>> GetAllRegionsAsync(GetAllRegionQuery query)
    {
        var response = await _clientService.GetAllRegionsAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<RegionsSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve regions"));
        }
        var regions = response.Data.Data.Select(region => new RegionsSanitized(Region: _referenceDataMappingService.GetOrCreateGuidForReferenceId(region.Id, Constants.Region),
                                                                                Label: region.Label, Logo: region.Logo, Map: region.Map)
        {
            InternalId = region.Id
        });
        return Result<IEnumerable<RegionsSanitized>>.Success(regions);
    }

    public async Task<Result<IEnumerable<ArrondissementSanitized>>> GetArrondissementsAsync(GetArrondissementQuery query)
    {
        var response = await _clientService.GetArrondissementsAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<ArrondissementSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve arrondissments"));
        }
        var arrondissments = response.Data.Data.Select(arr => new ArrondissementSanitized(Arrondissement: _referenceDataMappingService.GetOrCreateGuidForReferenceId(arr.Id, Constants.Arrondissement),
                                                                                            Label: arr.Label)
        {
            InternalId = arr.Id
        });
        return Result<IEnumerable<ArrondissementSanitized>>.Success(arrondissments);
    }

    public async Task<Result<IEnumerable<CommercialCuttingSanitized>>> GetCommercialCuttingAsync(GetCommercialCuttingQuery query)
    {
        var response = await _clientService.GetCommercialCuttingAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<CommercialCuttingSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Commercial Cuttings"));
        }
        var commercialCuttings = response.Data.Data.Select(cc => new CommercialCuttingSanitized(CommercialCutting: _referenceDataMappingService.GetOrCreateGuidForReferenceId(cc.Id, Constants.CommercailCut),
                                                                                                Label: cc.Label)
        {
            InternalId = cc.Id
        });
        return Result<IEnumerable<CommercialCuttingSanitized>>.Success(commercialCuttings);
    }

    public async Task<Result<IEnumerable<DemandTypeSanitized>>> GetDemandsTypeAsync(GetDemandTypesQuery query)
    {
        var response = await _clientService.GetDemandsTypeAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<DemandTypeSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Demand Tyoes"));
        }
        var demandTypes = response.Data.Data.Select(dt => new DemandTypeSanitized(DemandType: _referenceDataMappingService.GetOrCreateGuidForReferenceId(dt.Id, Constants.Demand),
                                                                                    Label: dt.Label, Description: dt.Description)
        {
            InternalId = dt.Id
        });
        return Result<IEnumerable<DemandTypeSanitized>>.Success(demandTypes);
    }

    public async Task<Result<IEnumerable<PartnerTypeSanitized>>> GetPartnerTypesAsync(GetPartnerTypesQuery query)
    {
        var response = await _clientService.GetPartnerTypesAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<PartnerTypeSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Partner Types"));
        }
        var partnerTypes = response.Data.Data.Select(pt => new PartnerTypeSanitized(PartnerType: _referenceDataMappingService.GetOrCreateGuidForReferenceId(pt.Id, Constants.Partner),
                                                                                    Label: pt.Label, Description: pt.Description)
        {
            InternalId = pt.Id
        });
        return Result<IEnumerable<PartnerTypeSanitized>>.Success(partnerTypes);
    }

    public async Task<Result<IEnumerable<TypeBienSanitized>>> GetTypeBienAsync(GetTypeBienQuery query)
    {
        var response = await _clientService.GetTypeBienAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<TypeBienSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Type Bien"));
        }
        var types = response.Data.Data.Select(tb => new TypeBienSanitized(TypeBien: _referenceDataMappingService.GetOrCreateGuidForReferenceId(tb.Id, Constants.Type),
                                                                            Label: tb.Label)
        {
            InternalId = tb.Id
        });
        return Result<IEnumerable<TypeBienSanitized>>.Success(types);
    }

    async Task<Result<IEnumerable<PackSanitized>>> IReferenceAglouDataService.GetAllPacksAsync(GetAllPackQuery query)
    {
        var response = await _clientService.GetAllPacksAsync();
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<IEnumerable<PackSanitized>>.Failure(new Error(Constants.ReferenceFail, "Failed to retrieve Packs"));
        }
        var packs = response.Data.Data.Select(pack => new PackSanitized(Pack: _referenceDataMappingService.GetOrCreateGuidForReferenceId(pack.Id, Constants.Pack),
                                                                        Label: pack.Label, ArabicLabel: pack.ArabicLabel, Description: pack.Description)
        {
            InternalId = pack.Id
        });
        return Result<IEnumerable<PackSanitized>>.Success(packs);
    }
}
