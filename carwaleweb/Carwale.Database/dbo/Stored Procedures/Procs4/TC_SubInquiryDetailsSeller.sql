IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SubInquiryDetailsSeller]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SubInquiryDetailsSeller]
GO

	
-- =============================================
-- Created By: Manish Chourasiya
-- Created Date:13-03-2013
-- Description: DashBoard Reports for Used Car sell Inquiries below are the report ids for parametre    
-- -- 1.Total inquiries   
-- 2. Followup inquiries  
--3. Closed inquiries  
--4.Converted inquiries  
--5. Archived
-- Modified By: Vivek Gupta on 29th, March,2013 : Added Makeyear,Price,Kms and Colour in each select statement
-- Modified By Vivek Gupta on 24-02-2014, Added join with inquirysource 
-- Tejashree Patil : 16 Jun 2014 Fetched TC_LeadStageId, TCIL.TC_UserId.
-- Tejashree Patil : 2 July 2014 Fetched IsTDRequested,TDRequestedDate.
--Vicky Gupta : 21 July 2015 Fetched Next follow-up date and time in @ReportId=2.
--Afrose on 29th Sept 2015, fetched Eagerness and last call comments, modified source for other sources specifically
--Modified by :Ashwini Dhamankar on Dec 15,2015 (partitioned data by TC_InquiriesLeadId of TC_SellerInquiries table and picked latest inquiry)
--Modified By : Ashwini Dhamankar on April 5,2016 (Fetched owners,IsSychronizedCW and sellInquiryId)
--exec TC_SubInquiryDetailsSeller 5,'2016-5-1', '2016-5-5', 1
-- Modified BY: Tejashree Patil on 22 April 2016, Joined with CustomerSellInquiryDetails.
-- Modified by: Kritika Choudhary on 5th May 2016 added TCBI.CWInquiryId  and join with livelistings 
-- Modified BY: Tejashree Patil on 11 Nov 2016, fetched CWInquiryId in case of @ReportId = 1.
--=============================================
CREATE PROCEDURE [dbo].[TC_SubInquiryDetailsSeller] (
    @BranchId INT
    ,@FromDate DATETIME
    ,@ToDate DATETIME
    ,@ReportId TINYINT
    )
AS
BEGIN
    IF @ReportId = 1
    BEGIN
        WITH CTE0
        AS (
            SELECT DISTINCT TC.Id
                ,TC.CustomerName
                ,TC.Email
                ,TC.Mobile
                ,TC.Location
                ,year(TCBI.MakeYear) AS MakeYear
                ,TCBI.Price
                ,TCBI.Kms
                ,TCBI.Colour
                ,TCBI.CreatedOn AS EntryDate
                ,TU.UserName
                ,TCIL.TC_InquiryStatusId AS Eagerness
                ,--Added by Afrose
                TCAC.LastCallComment AS Comment
                ,--Added by Afrose
                vwMMV.Car
                ,CASE TCIS.Source
                    WHEN 'Other Sources'
                        THEN 'Other Sources-' + ' ' + (
                                SELECT SourceName
                                FROM TC_OtherInquirySources WITH (NOLOCK)
                                WHERE InquiryId = TCBI.TC_SellerInquiriesId
                                )
                    ELSE TCIS.Source
                    END AS Source
                ,--Modified by Afrose
                TL.TC_LeadId
                ,TCIL.TC_UserId
                ,TL.TC_LeadStageId
                ,0 AS IsTDRequested
                ,'' AS TDRequestedDate
                ,'' AS NextFollowUpDT
                ,CASE 
                    WHEN TCBI.CWInquiryId IS NOT NULL
                        THEN ISNULL(cast(TU.UserName AS VARCHAR(20)), 0)
                    ELSE ISNULL(TCBI.Owners, 0)
                    END AS Owners
                ,
                --ISNULL(CSD.Owners,0) AS Owners,
                CASE 
                    WHEN ISNULL(ST.IsSychronizedCW, 0) = 1
                        THEN 'Yes'
                    ELSE 'No'
                    END AS IsSychronizedCW
                -- ,SI.Id AS SellInquiryId
                ,ROW_NUMBER() OVER (
                    PARTITION BY TCBI.TC_InquiriesLeadId ORDER BY TCBI.CreatedOn DESC
                    ) AS ROW
            ,NULL AS CWInquiryId -- Modified BY: Tejashree Patil on 11 Nov 2016
            -- ,L.Inquiryid CWInquiryId -- Modified by: Kritika Choudhary on 5th May 2016 addedTCBI.CWInquiryId 
            FROM TC_CustomerDetails AS TC WITH (NOLOCK)
            JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_CustomerId = TC.Id
            JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
                AND TCIL.TC_LeadInquiryTypeId = 2
                AND TL.BranchId = @BranchId
                AND TCIL.BranchId = @BranchId
            JOIN TC_SellerInquiries AS TCBI WITH (NOLOCK) ON TCBI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
                AND TCBI.CreatedOn BETWEEN @FromDate
                    AND @ToDate
            LEFT OUTER JOIN TC_Users AS TU WITH (NOLOCK) ON TU.Id = TCIL.TC_UserId
            LEFT OUTER JOIN vwMMV WITH (NOLOCK) ON vwMMV.VersionId = TCBI.CarVersionId
            LEFT OUTER JOIN TC_InquirySource TCIS WITH (NOLOCK) ON TCBI.TC_InquirySourceId = TCIS.Id -- Modified By Vivek Gupta on 24-02-2014
            LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId = TL.TC_LeadId --Added by Afrose 
            LEFT JOIN TC_Stock AS ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = TCBI.TC_SellerInquiriesId
                -- LEFT JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = ST.Id
                -- LEFT JOIN livelistings L WITH (NOLOCK) ON L.Inquiryid = TCBI.CWInquiryId
                --AND L.SellerType = 2
                -- LEFT JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSD.InquiryId = TCBI.CWInquiryId
                --    ORDER BY TCBI.CreatedOn DESC   
            )
        SELECT *
        FROM CTE0
        WHERE ROW = 1
    END
    ELSE
        IF @ReportId = 2
        BEGIN
            WITH CTE
            AS (
                SELECT DISTINCT TC.Id
                    ,TC.CustomerName
                    ,TC.Email
                    ,TC.Mobile
                    ,TC.Location
                    ,year(TCBI.MakeYear) AS MakeYear
                    ,TCBI.Price
                    ,TCBI.Kms
                    ,TCBI.Colour
                    ,TCBI.CreatedOn AS EntryDate
                    ,TU.UserName
                    ,TCIL.TC_InquiryStatusId AS Eagerness
                    ,--Added by Afrose
                    TCAC.LastCallComment AS Comment
                    ,--Added by Afrose
                    vwMMV.Car
                    ,CASE TCIS.Source
                        WHEN 'Other Sources'
                            THEN 'Other Sources-' + ' ' + (
                                    SELECT SourceName
                                    FROM TC_OtherInquirySources WITH (NOLOCK)
                                    WHERE InquiryId = TCBI.TC_SellerInquiriesId
                                    )
                        ELSE TCIS.Source
                        END AS Source
                    ,--Modified by Afrose
                    TL.TC_LeadId
                    ,TCIL.TC_UserId
                    ,TL.TC_LeadStageId
                    ,0 AS IsTDRequested
                    ,'' AS TDRequestedDate
                    ,TCAC.ScheduledOn AS NextFollowUpDT
                    ,CASE 
                        WHEN TCBI.CWInquiryId IS NOT NULL
                            THEN ISNULL(cast(TU.UserName AS VARCHAR(20)), 0)
                        ELSE ISNULL(TCBI.Owners, 0)
                        END AS Owners
                    ,
                    --ISNULL(CSD.Owners,0) AS Owners,
                    CASE 
                        WHEN ISNULL(ST.IsSychronizedCW, 0) = 1
                            THEN 'Yes'
                        ELSE 'No'
                        END AS IsSychronizedCW
                    -- ,SI.Id AS SellInquiryId
                    ,ROW_NUMBER() OVER (
                        PARTITION BY TCBI.TC_InquiriesLeadId ORDER BY TCBI.CreatedOn DESC
                        ) AS ROW
                    ,NULL AS CWInquiryId -- Modified by: Kritika Choudhary on 5th May 2016 addedTCBI.CWInquiryId 
                FROM TC_CustomerDetails AS TC WITH (NOLOCK)
                JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_CustomerId = TC.Id
                JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
                    AND TCIL.TC_LeadInquiryTypeId = 2
                    AND TL.BranchId = @BranchId
                    AND TCIL.BranchId = @BranchId
                    AND (
                        TL.TC_LeadStageId = 1
                        OR TL.TC_LeadStageId = 2
                        )
                JOIN TC_SellerInquiries AS TCBI WITH (NOLOCK) ON TCBI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
                    AND TCBI.CreatedOn BETWEEN @FromDate
                        AND @ToDate
                    AND TCBI.TC_LeadDispositionId IS NULL
                LEFT OUTER JOIN TC_Users AS TU WITH (NOLOCK) ON TU.Id = TCIL.TC_UserId
                LEFT OUTER JOIN vwMMV WITH (NOLOCK) ON vwMMV.VersionId = TCBI.CarVersionId
                LEFT OUTER JOIN TC_InquirySource TCIS WITH (NOLOCK) ON TCBI.TC_InquirySourceId = TCIS.Id -- Modified By Vivek Gupta on 24-02-2014
                JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TL.TC_LeadId = TCAC.TC_LeadId --Modified by Vicky Gupta on 21-07-2015 
                LEFT JOIN TC_Stock AS ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = TCBI.TC_SellerInquiriesId
                    -- LEFT JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = ST.Id
                    -- LEFT JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSD.InquiryId = TCBI.CWInquiryId
                    --ORDER BY TCBI.CreatedOn DESC
                )
            SELECT *
            FROM CTE
            WHERE ROW = 1
        END
        ELSE
            IF @ReportId = 3
            BEGIN
                WITH CTE1
                AS (
                    SELECT DISTINCT TC.Id
                        ,TC.CustomerName
                        ,TC.Email
                        ,TC.Mobile
                        ,TC.Location
                        ,year(TCBI.MakeYear) AS MakeYear
                        ,TCBI.Price
                        ,TCBI.Kms
                        ,TCBI.Colour
                        ,TCBI.CreatedOn AS EntryDate
                        ,TU.UserName
                        ,TCIL.TC_InquiryStatusId AS Eagerness
                        ,--Added by Afrose
                        TCAC.LastCallComment AS Comment
                        ,--Added by Afrose
                        vwMMV.Car
                        ,TCLD.NAME AS [Reason for Close]
                        ,CASE TCIS.Source
                            WHEN 'Other Sources'
                                THEN 'Other Sources-' + ' ' + (
                                        SELECT SourceName
                                        FROM TC_OtherInquirySources WITH (NOLOCK)
                                        WHERE InquiryId = TCBI.TC_SellerInquiriesId
                                        )
                            ELSE TCIS.Source
                            END AS Source
                        ,--Modified by Afrose
                        TL.TC_LeadId
                        ,TCIL.TC_UserId
                        ,TL.TC_LeadStageId
                        ,0 AS IsTDRequested
                        ,'' AS TDRequestedDate
                        ,'' AS NextFollowUpDT
                        ,CASE 
                            WHEN TCBI.CWInquiryId IS NOT NULL
                                THEN ISNULL(cast(TU.UserName AS VARCHAR(20)), 0)
                            ELSE ISNULL(TCBI.Owners, 0)
                            END AS Owners
                        ,
                        --ISNULL(CSD.Owners,0) AS Owners,
                        CASE 
                            WHEN ISNULL(ST.IsSychronizedCW, 0) = 1
                                THEN 'Yes'
                            ELSE 'No'
                            END AS IsSychronizedCW
                        -- ,SI.Id AS SellInquiryId
                        ,ROW_NUMBER() OVER (
                            PARTITION BY TCBI.TC_InquiriesLeadId ORDER BY TCBI.CreatedOn DESC
                            ) AS ROW
                        ,NULL AS CWInquiryId -- Modified by: Kritika Choudhary on 5th May 2016 addedTCBI.CWInquiryId 
                    FROM TC_CustomerDetails AS TC WITH (NOLOCK)
                    JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_CustomerId = TC.Id
                    JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
                        AND TCIL.TC_LeadInquiryTypeId = 2
                        AND TL.BranchId = @BranchId
                        AND TCIL.BranchId = @BranchId
                    -- AND  TL.TC_LeadStageId=3
                    JOIN TC_SellerInquiries AS TCBI WITH (NOLOCK) ON TCBI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
                        AND TCBI.CreatedOn BETWEEN @FromDate
                            AND @ToDate
                        AND (
                            (
                                TL.TC_LeadDispositionId = 3
                                OR TL.TC_LeadDispositionId = 1
                                OR TCBI.TC_LeadDispositionId IS NOT NULL
                                )
                            AND (
                                TCBI.TC_LeadDispositionId <> 4
                                OR TCBI.TC_LeadDispositionId IS NULL
                                )
                            )
                    JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON ISNULL(TCBI.TC_LeadDispositionId, TL.TC_LeadDispositionId) = TCLD.TC_LeadDispositionId
                    LEFT OUTER JOIN TC_Users AS TU WITH (NOLOCK) ON TU.Id = TCIL.TC_UserId
                    LEFT OUTER JOIN vwMMV WITH (NOLOCK) ON vwMMV.VersionId = TCBI.CarVersionId
                    LEFT OUTER JOIN TC_InquirySource TCIS WITH (NOLOCK) ON TCBI.TC_InquirySourceId = TCIS.Id -- Modified By Vivek Gupta on 24-02-2014
                    LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId = TL.TC_LeadId --Added by Afrose 
                    LEFT JOIN TC_Stock AS ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = TCBI.TC_SellerInquiriesId
                        -- LEFT JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = ST.Id
                        -- LEFT JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSD.InquiryId = TCBI.CWInquiryId
                    )
                SELECT *
                FROM CTE1
                WHERE ROW = 1
            END
            ELSE
                IF @ReportId = 4
                BEGIN
                    WITH CTE2
                    AS (
                        SELECT DISTINCT TC.Id
                            ,TC.CustomerName
                            ,TC.Email
                            ,TC.Mobile
                            ,TC.Location
                            ,year(TCBI.MakeYear) AS MakeYear
                            ,TCBI.Price
                            ,TCBI.Kms
                            ,TCBI.Colour
                            ,TCBI.CreatedOn AS EntryDate
                            ,TU.UserName
                            ,TCIL.TC_InquiryStatusId AS Eagerness
                            ,--Added by Afrose
                            TCAC.LastCallComment AS Comment
                            ,--Added by Afrose
                            vwMMV.Car
                            ,CASE TCIS.Source
                                WHEN 'Other Sources'
                                    THEN 'Other Sources-' + ' ' + (
                                            SELECT SourceName
                                            FROM TC_OtherInquirySources WITH (NOLOCK)
                                            WHERE InquiryId = TCBI.TC_SellerInquiriesId
                                            )
                                ELSE TCIS.Source
                                END AS Source
                            ,--Modified by Afrose
                            TL.TC_LeadId
                            ,TCIL.TC_UserId
                            ,TL.TC_LeadStageId
                            ,0 AS IsTDRequested
                            ,'' AS TDRequestedDate
                            ,'' AS NextFollowUpDT
                            ,CASE 
                                WHEN TCBI.CWInquiryId IS NOT NULL
                                    THEN ISNULL(cast(TU.UserName AS VARCHAR(20)), 0)
                                ELSE ISNULL(TCBI.Owners, 0)
                                END AS Owners
                            ,
                            --ISNULL(CSD.Owners,0) AS Owners,
                            CASE 
                                WHEN ISNULL(ST.IsSychronizedCW, 0) = 1
                                    THEN 'Yes'
                                ELSE 'No'
                                END AS IsSychronizedCW
                            -- ,SI.Id AS SellInquiryId
                            ,ROW_NUMBER() OVER (
                                PARTITION BY TCBI.TC_InquiriesLeadId ORDER BY TCBI.CreatedOn DESC
                                ) AS ROW
                            ,NULL AS CWInquiryId -- Modified by: Kritika Choudhary on 5th May 2016 addedTCBI.CWInquiryId 
                        FROM TC_CustomerDetails AS TC WITH (NOLOCK)
                        JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_CustomerId = TC.Id
                        JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
                            AND TCIL.TC_LeadInquiryTypeId = 2
                            AND TL.BranchId = @BranchId
                            AND TCIL.BranchId = @BranchId
                        JOIN TC_SellerInquiries AS TCBI WITH (NOLOCK) ON TCBI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
                            AND TCBI.CreatedOn BETWEEN @FromDate
                                AND @ToDate
                            AND TCBI.TC_LeadDispositionId = 4
                        LEFT OUTER JOIN TC_Users AS TU WITH (NOLOCK) ON TU.Id = TCIL.TC_UserId
                        LEFT OUTER JOIN vwMMV WITH (NOLOCK) ON vwMMV.VersionId = TCBI.CarVersionId
                        LEFT OUTER JOIN TC_InquirySource TCIS WITH (NOLOCK) ON TCBI.TC_InquirySourceId = TCIS.Id -- Modified By Vivek Gupta on 24-02-2014
                        LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId = TL.TC_LeadId --Added by Afrose 
                        LEFT JOIN TC_Stock AS ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = TCBI.TC_SellerInquiriesId
                            -- LEFT JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = ST.Id
                            -- LEFT JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSD.InquiryId = TCBI.CWInquiryId
                            --ORDER BY TCBI.CreatedOn DESC
                        )
                    SELECT *
                    FROM CTE2
                    WHERE ROW = 1
                END
                ELSE
                    IF @ReportId = 5
                    BEGIN
                        WITH CTE3
                        AS (
                            SELECT DISTINCT TC.Id
                                ,TC.CustomerName
                                ,TC.Email
                                ,TC.Mobile
                                ,TC.Location
                                ,year(TCBI.MakeYear) AS MakeYear
                                ,TCBI.Price
                                ,TCBI.Kms
                                ,TCBI.Colour
                                ,TCBI.CreatedOn AS EntryDate
                                ,TU.UserName
                                ,TCIL.TC_InquiryStatusId AS Eagerness
                                ,--Added by Afrose
                                TCAC.LastCallComment AS Comment
                                ,--Added by Afrose
                                vwMMV.Car
                                ,CASE TCIS.Source
                                    WHEN 'Other Sources'
                                        THEN 'Other Sources-' + ' ' + (
                                                SELECT SourceName
                                                FROM TC_OtherInquirySources WITH (NOLOCK)
                                                WHERE InquiryId = TCBI.TC_SellerInquiriesId
                                                )
                                    ELSE TCIS.Source
                                    END AS Source
                                ,--Modified by Afrose
                                TL.TC_LeadId
                                ,TCIL.TC_UserId
                                ,TL.TC_LeadStageId
                                ,0 AS IsTDRequested
                                ,'' AS TDRequestedDate
                                ,'' AS NextFollowUpDT
                                ,CASE 
                                    WHEN TCBI.CWInquiryId IS NOT NULL
                                        THEN ISNULL(cast(TU.UserName AS VARCHAR(20)), 0)
                                    ELSE ISNULL(TCBI.Owners, 0)
                                    END AS Owners
                                ,
                                --ISNULL(CSD.Owners,0) AS Owners,
                                CASE 
                                    WHEN ISNULL(ST.IsSychronizedCW, 0) = 1
                                        THEN 'Yes'
                                    ELSE 'No'
                                    END AS IsSychronizedCW
                                -- ,SI.Id AS SellInquiryId
                                ,ROW_NUMBER() OVER (
                                    PARTITION BY TCBI.TC_InquiriesLeadId ORDER BY TCBI.CreatedOn DESC
                                    ) AS ROW
                                ,NULL AS CWInquiryId -- Modified by: Kritika Choudhary on 5th May 2016 addedTCBI.CWInquiryId 
                            FROM TC_CustomerDetails AS TC WITH (NOLOCK)
                            JOIN TC_Lead AS TL WITH (NOLOCK) ON TL.TC_CustomerId = TC.Id
                            JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
                                AND TCIL.TC_LeadInquiryTypeId = 2
                                AND TL.BranchId = @BranchId
                                AND TCIL.BranchId = @BranchId
                                AND TL.TC_LeadDispositionId = 41
                            JOIN TC_SellerInquiries AS TCBI WITH (NOLOCK) ON TCBI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
                                AND TCBI.CreatedOn BETWEEN @FromDate
                                    AND @ToDate
                            LEFT OUTER JOIN TC_Users AS TU WITH (NOLOCK) ON TU.Id = TCIL.TC_UserId
                            LEFT OUTER JOIN vwMMV WITH (NOLOCK) ON vwMMV.VersionId = TCBI.CarVersionId
                            LEFT OUTER JOIN TC_InquirySource TCIS WITH (NOLOCK) ON TCBI.TC_InquirySourceId = TCIS.Id -- Modified By Vivek Gupta on 24-02-2014
                            LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId = TL.TC_LeadId --Added by Afrose 
                            LEFT JOIN TC_Stock AS ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = TCBI.TC_SellerInquiriesId
                                -- LEFT JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.TC_StockId = ST.Id
                                -- LEFT JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK) ON CSD.InquiryId = TCBI.CWInquiryId
                                --    ORDER BY TCBI.CreatedOn DESC
                            )
                        SELECT *
                        FROM CTE3
                        WHERE ROW = 1
                    END
END
    -------------------------------------------