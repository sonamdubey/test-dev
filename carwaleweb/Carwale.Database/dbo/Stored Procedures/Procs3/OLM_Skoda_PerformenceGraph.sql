IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_Skoda_PerformenceGraph]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_Skoda_PerformenceGraph]
GO

	

CREATE PROCEDURE [dbo].[OLM_Skoda_PerformenceGraph]
	
	
	(
	@RegionId	int,
	@DealerId	int,
	@CityId		int,
	@ModelId	int,
    @YearFrom	int,
    @YearTo		int,
    @MonthFrom	int,
    @MonthTo	int,
    @DayFrom	int,
    @DayTo		int,
    @DateType   int,
	@Top		int,
	@Bottom		int
	)
	AS
	BEGIN  -- here we will finaly get output table have three colums with two data columns and one date column
		 	SELECT * INTO #performencetbl     
	FROM
		(SELECT ISNULL(T1.TotalLeads,0) AS LMSLead,ISNULL(T2.TotalLeads,0) AS BookedLead,COALESCE(T1.DateVal,T2.DateVal)As DateVal, 
                COALESCE(T1.RegionId,T2.RegionId)As RegionId,COALESCE(T1.Region,T2.Region)As Region,COALESCE(T1.Name,T2.Name)As Name, 
                COALESCE(T1.OrgId,T2.OrgId)AS Id 
		 FROM 
            (SELECT COUNT(DISTINCT CPD.LeadId) As TotalLeads,CL.CreatedOnDatePart AS DateVal,OC.RegionId,ORG.Region,NSDO.Id AS OrgId,NSDO.Name 
             FROM CRM_PrePushData AS CPD, CRM_Customers AS CC, CRM_CarBasicData AS CBD,OLM_RegionCities AS OC,OLM_Regions AS ORG, 
                CRM_SkodaDealerAssignment AS CSD,NCS_Dealers AS ND,NCS_SubDealerOrganization AS NSD, NCS_DealerOrganization AS NSDO,CRM_Leads AS CL 
               	LEFT JOIN CRM_LeadSource AS CLS ON CLS.LeadId = CL.ID 
             WHERE CL.ID = CBD.LeadId AND CL.CNS_CustId = CC.Id AND CPD.Result = 'SUCCESS' 
               	AND CPD.LeadId = CL.Id AND CC.CityId =OC.CityId AND ND.ID=NSD.DId AND NSDO.ID=NSD.OId 
               	AND OC.RegionId =ORG.Id AND ORG.IsActive=1 AND ORG.MakeId=15  AND CL.ID=CSD.LeadId AND CSD.DealerId= ND.ID 
               	AND YEAR(CL.CreatedOnDatePart)between @YearFrom AND @YearTo AND MONTH(CL.CreatedOnDatePart) between @MonthFrom AND @MonthTo 
                AND(@DateType <> 1 Or (DAY(CL.CreatedOnDatePart) between @DayFrom and @DayTo) ) 
               	AND (@DealerId = 0 Or (ND.ID=@DealerId)) AND (@CityId =0 OR (CC.CityId =@CityId)) AND OC.RegionId=@RegionId 
               	AND (@ModelId = -1 Or (CBD.VersionId IN(SELECT Id FROM CarVersions WHERE CarModelId = @ModelId))) 
            GROUP BY CL.CreatedOnDatePart,OC.RegionId,ORG.Region,NSDO.Id,NSDO.Name) AS T1 
       	 FULL OUTER JOIN 
           	( SELECT COUNT(DISTINCT CI.CBDId) TotalLeads,OC.RegionId,ORG.Region,CAI.InvoiceMonth AS DateVal,CII.ClosingProbability,NSDO.Id AS OrgId,NSDO.Name 
              FROM CRM_CarInvoices AS CI, CRM_ADM_Invoices AS CAI, CRM_Leads AS CL, NCS_Dealers AS ND, 
                CRM_InterestedIn AS CII,OLM_RegionCities AS OC,OLM_Regions AS ORG,NCS_SubDealerOrganization AS NSD, NCS_DealerOrganization AS NSDO,CRM_CarDealerAssignment AS CDA, 
                CRM_CarBasicData AS CBD LEFT JOIN CRM_LeadSource AS CLS ON CBD.LeadId = CLS.LeadId 
              WHERE CI.InvoiceId = CAI.Id AND CI.IsActive = 1 AND CAI.MakeId  = 15 AND CI.CBDId = CBD.ID AND CBD.LeadId = CII.LeadId 
               	 AND ND.ID=NSD.DId AND NSDO.ID=NSD.OId 
               	 AND CII.ProductTypeId = 1 AND CDA.CBDId = CI.CBDId AND CBD.LeadId = CL.ID AND CDA.DealerId =  ND.Id AND ND.CityId = OC.CityId  
               	 AND OC.RegionId =ORG.Id AND ORG.IsActive=1 AND ORG.MakeId=15  
                 AND YEAR(CL.CreatedOnDatePart) between @YearFrom AND @YearTo AND MONTH(CL.CreatedOnDatePart)between @MonthFrom AND @MonthTo 
                 AND(@DateType <> 1 Or (DAY(CL.CreatedOnDatePart) between @DayFrom and @DayTo) ) 
                 AND(@DealerId = 0 Or (ND.ID=@DealerId))AND (@CityId=0 OR (OC.CityId=@CityId)) AND OC.RegionId= @RegionId 
                 AND (@ModelId = -1 Or (CBD.VersionId IN(SELECT Id FROM CarVersions WHERE CarModelId = @ModelId))) 
             GROUP BY CAI.InvoiceMonth, CII.ClosingProbability,OC.RegionId,ORG.Region,NSDO.Id,NSDO.Name) AS T2 
         ON T1.OrgId=T2.OrgId AND T1.DateVal=T2.DateVal)As tbl 
         
         
       SELECT COALESCE(T1.DateVal,T2.DateVal) AS DateVal, ISNULL(T1.RatioTop,0) AS RatioTop ,ISNULL(T2.RatioBottom,0) AS RatioBottom ,
			((ISNULL(T1.RatioTop,0)+ ISNULL(T2.RatioBottom,0))/2) AS RatioAvg
       FROM
			(SELECT (BookedLead *100/(case LMSLead when 0 then 1 else LMSLead end))As RatioTop,BookedLead,LMSLead,DateVal,DAY(DateVal)AS DayVal,Month(DateVal)
				AS MonthVal,Year(DateVal)AS YearVal FROM #performencetbl WHERE ID=@Top) AS T1
       FULL OUTER JOIN 
			(SELECT (BookedLead *100/(case LMSLead when 0 then 1 else LMSLead end))As RatioBottom,BookedLead,LMSLead,DateVal,DAY(DateVal)AS DayVal,Month(DateVal)
				AS MonthVal,Year(DateVal)AS YearVal FROM #performencetbl WHERE ID=@Bottom) AS T2
	   ON T1.DateVal=T2.DateVal ORDER BY DateVal
       
       DROP TABLE #performencetbl 

	    
		
	END


