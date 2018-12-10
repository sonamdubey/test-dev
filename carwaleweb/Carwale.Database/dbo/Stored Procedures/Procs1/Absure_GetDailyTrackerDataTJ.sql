IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetDailyTrackerDataTJ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetDailyTrackerDataTJ]
GO

	CREATE Procedure [dbo].[Absure_GetDailyTrackerDataTJ]
as
begin

        declare @ReportDate datetime=getdate()-1
        declare @Trackerid int

        ----Inspection Request-----And MTD nos----
        insert into Absure_Dailytracker(InspectionRequests)
        SELECT  distinct count(Id) as InspectionRequest FROM AbSure_CarDetails with (nolock) 
        WHERE CONVERT(DATE,EntryDate) = convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)     
        and CancelReason is null 
        and ISNULL(Status,0) <> 3

        select @Trackerid = SCOPE_IDENTITY()

        update Absure_Dailytracker
        set InspectionRequestsMTD=
        (SELECT  distinct count(Id) as InspectionRequestMTD FROM AbSure_CarDetails with (nolock) 
        WHERE CONVERT(DATE,EntryDate) <= convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        and CancelReason is null
        and ISNULL(Status,0) <> 3)
        where  Trackerid=@Trackerid

        -----------------------------------------------------------------------------------
        ------allocation-----

        update Absure_Dailytracker
        set Allocations=(
        SELECT  distinct count(ad.Id) as allocationRequest FROM AbSure_CarSurveyorMapping ab with (nolock)
        join AbSure_CarDetails ad with (nolock) on ab.AbSure_CarDetailsId=ad.Id
        --join TC_Users U with (nolock) on U.Id = ab.TC_UserId and U.IsAgency = 0--surveyor
        WHERE CONVERT(DATE,ab.EntryDate) = convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        and ad.CancelReason  is null)
        where  Trackerid=@Trackerid


        update Absure_Dailytracker
        set AllocationsMTD=(
        SELECT  distinct count(ad.Id) as allocationRequestMTD FROM AbSure_CarSurveyorMapping ab with (nolock)
        left join AbSure_CarDetails ad with (nolock) on ab.AbSure_CarDetailsId=ad.Id
        WHERE CONVERT(DATE,ab.EntryDate) <= convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        and ad.CancelReason  is null
        )
        where  Trackerid=@Trackerid
        -----------------------------------------------------------------------

        ----- Inspections Done-----

        update Absure_Dailytracker
        set InspectionsDone=(
        SELECT distinct COUNT(IsSurveyDone)as inspectiondone FROM AbSure_CarDetails ad with (nolock) 
        WHERE CONVERT(DATE,SurveyDate) =convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        AND IsSurveyDone IS NOT NULL 
        and CancelReason is null
        and ISNULL(Status,0) <> 3
        )
        where  Trackerid=@Trackerid

        update Absure_Dailytracker
        set InspectionsDoneMTD=(
        SELECT distinct COUNT(IsSurveyDone)as inspectiondoneMTD FROM AbSure_CarDetails ad with (nolock) 
        WHERE CONVERT(DATE,SurveyDate) <=convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        AND IsSurveyDone IS NOT NULL 
        and CancelReason is null
        and ISNULL(Status,0) <> 3
        )
        where  Trackerid=@Trackerid
        -------------------------------------------------------------------------------------->

        -------Eligible for Warranty-----
        update Absure_Dailytracker
        set EligibleforWarranty=
        (
        SELECT  COUNT(AbSure_WarrantyTypesId) as eligbleWarranty FROM AbSure_CarDetails ad with (nolock) 
        WHERE CONVERT(DATE,SurveyDate) =convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        AND AbSure_WarrantyTypesId IS NOT NULL 
        and CancelReason is null
        and ISNULL(Status,0) <> 3
        AND ISNULL(ad.IsRejected,0) = 0
        --GROUP BY AbSure_WarrantyTypesId
        )
        where  Trackerid=@Trackerid


        update Absure_Dailytracker
        set EligibleforWarrantyMTD=(
        SELECT  COUNT(AbSure_WarrantyTypesId) as eligbleWarrantyMTD FROM AbSure_CarDetails ad with (nolock) 
        WHERE CONVERT(DATE,SurveyDate) <=convert(date,@ReportDate)
        and dealerid not in (11392,11383,4271,3838)   
        AND AbSure_WarrantyTypesId IS NOT NULL and CancelReason is null and ISNULL(Status,0) <> 3)
        --GROUP BY AbSure_WarrantyTypesId
        where  Trackerid=@Trackerid
        ---------------------------------------------------------------------------------------------------->

        ---------Warranties Approved-----------
        update Absure_Dailytracker
        set WarrantiesApproved=(
        SELECT  COUNT(FinalWarrantyTypeId)as warrantyapproved FROM AbSure_CarDetails ad with (nolock) 
        WHERE 
        CONVERT(DATE,FinalWarrantyDate)
        =convert(date,@ReportDate)
         and ad.dealerid not in (11392,11383,4271,3838)   
        and FinalWarrantyTypeId IS NOT NULL and CancelReason is null and ISNULL(Status,0) <> 3
        --GROUP BY IsSurveyDone,AbSure_WarrantyTypesId,FinalWarrantyTypeId
        )
        where  Trackerid=@Trackerid


        update Absure_Dailytracker
        set WarrantiesApprovedMTD=
        (
        SELECT  COUNT(FinalWarrantyTypeId)as warrantyapprovedMTD FROM AbSure_CarDetails ad with (nolock) 
        WHERE 
        CONVERT(DATE,FinalWarrantyDate)
        <=convert(date,@ReportDate)
        and ad.dealerid not in (11392,11383,4271,3838)   
        and FinalWarrantyTypeId IS NOT NULL and CancelReason is null and ISNULL(Status,0) <> 3)
        where  Trackerid=@Trackerid
        --GROUP BY IsSurveyDone,AbSure_WarrantyTypesId,FinalWarrantyTypeId

        ------------------------------------------------------------------------------------------------->

        ----pending for allocation (Requested Till 17th Mar'15)----Only MTD pls check getdate ?
        update Absure_Dailytracker
        set PendingForAllocation=(
        select 
        COUNT(distinct a.id) [pending for allocation]
        --a.*,b.absure_carsurveyormappingid
        from
        (select * FROM AbSure_CarDetails with (nolock)
        where entrydate <= GETDATE()-5
        and cancelledby is null
        and dealerid not in (11392,11383,4271,3838)   
        )a 
        --order by entrydate desc
        left outer join
        (select * from absure_carsurveyormapping with (nolock))b
        on a.id = b.absure_cardetailsid
        where b.absure_carsurveyormappingid is null
        )
        where  Trackerid=@Trackerid
        ---------------------------------------------------------------------------------------------

        --- pending for inspection (Allocated Till 17th Mar'15)--Only MTD and pls check getdate ?

        update Absure_Dailytracker
        set pendingforinspection=(
        select 
        count(distinct a.absure_cardetailsid) [pending for inspection]
        --,
        --count(distinct b.id)
        --a.*,b.issurveydone
        from
        (select * from AbSure_CarSurveyorMapping with (nolock)
        where entrydate <= getdate()-5
        )a
        left outer join
        (select * FROM AbSure_CarDetails with (nolock))b
        on a.absure_cardetailsid = b.id
        where IsSurveyDone is null
        and dealerid not in (11392,11383,4271,3838)   
        and b.CancelReason is null
        and ISNULL(Status,0) <> 3
        )
        where  Trackerid=@Trackerid

        ---------------------------------------------------------------------------------------->

        ---------------Inspections Requested Before 4pm------------------pls check getdate ?

        update Absure_Dailytracker
        set InspectionsRequestedBefore4pm=(
        SELECT
        ---id,  datepart(hour,entrydate)
        distinct count(Id)
         as Request FROM AbSure_CarDetails with (nolock) 
        WHERE CONVERT(DATE,EntryDate) = convert(date,@ReportDate) and datepart(hour,entrydate)<16
        and dealerid not in (11392,11383,4271,3838)   
        and CancelReason is null
        and ISNULL(Status,0) <> 3
        )
        where  Trackerid=@Trackerid

        --------------------------------------------------------------------------------->

        -------------Pending For Inspection (Allocated Before 17th Mar'15 - 4pm)Only MTD-------------pls changes date ?

        update Absure_Dailytracker
        set PendingForInspection4pm=(
        select 
        count(distinct a.absure_cardetailsid)
        --,
        --count(distinct b.id)
        --a.*,b.issurveydone
        from
        (select * from AbSure_CarSurveyorMapping 
        where
        EntryDate < getdate()-4 and datepart(hour,entrydate) <16
        --entrydate <= '2015-03-20 16:00:00'
        --order by entrydate desc

        )a
        left outer join
        (select * FROM AbSure_CarDetails with (nolock))b
        on a.absure_cardetailsid = b.id
        where IsSurveyDone is null
        and dealerid not in (11392,11383,4271,3838)   
        and b.CancelReason is null
        and ISNULL(Status,0) <> 3
        )
        where  Trackerid=@Trackerid
        -------------------------------------------------------------------------------------------------->

end