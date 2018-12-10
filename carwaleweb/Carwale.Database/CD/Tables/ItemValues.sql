CREATE TABLE [CD].[ItemValues] (
    [ItemValueId]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [CarVersionId]  NUMERIC (18)  NULL,
    [ItemMasterId]  BIGINT        NULL,
    [DataTypeId]    SMALLINT      NULL,
    [ItemValue]     FLOAT (53)    NULL,
    [UserDefinedId] INT           NULL,
    [CustomText]    VARCHAR (200) NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_ItemValues_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_ItemValueId] PRIMARY KEY CLUSTERED ([ItemValueId] ASC),
    CONSTRAINT [FK_ItemValues_DataTypes] FOREIGN KEY ([DataTypeId]) REFERENCES [CD].[DataTypes] ([DataTypeId]),
    CONSTRAINT [FK_ItemValues_ItemMaster] FOREIGN KEY ([ItemMasterId]) REFERENCES [CD].[ItemMaster] ([ItemMasterId]),
    CONSTRAINT [FK_ItemValues_UserDefinedMaster] FOREIGN KEY ([UserDefinedId]) REFERENCES [CD].[UserDefinedMaster] ([UserDefinedId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ItemValues]
    ON [CD].[ItemValues]([CarVersionId] ASC, [ItemMasterId] ASC);


GO
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <4/2/2013>
-- Description:	<Updates the Flg IsSpecsAvailable in Car Versions table whenever a new version is added in the ItemValues table or deleted from it.>
--Modified By: Reshma Shetty -Also update the IsSpecsExist field
-- =============================================
CREATE TRIGGER [CD].[TRG_AID_IVCarVersionId] 
   ON  [CD].[ItemValues]
   AFTER INSERT,DELETE 
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CarVersionId INT ;
	DECLARE @Bit INT ;
	
	
	SELECT @CarVersionId = CarVersionId
		,@Bit = 1
	FROM Inserted
	WHERE CarVersionId IS NOT NULL

	SELECT @CarVersionId = CarVersionId
		,@Bit = 0
	FROM Deleted
	WHERE CarVersionId IS NOT NULL
		AND CarVersionId NOT IN (
			SELECT DISTINCT CarVersionID
			FROM [CD].[ItemValues] WITH(NOLOCK)
			)

	UPDATE CarVersions
	SET IsSpecsAvailable = @Bit,IsSpecsExist=@Bit
	WHERE ID = @CarVersionId
		AND IsSpecsAvailable <> @Bit

    -- Insert statements for trigger here

END

GO
DISABLE TRIGGER [CD].[TRG_AID_IVCarVersionId]
    ON [CD].[ItemValues];


GO
-- =============================================
-- Author:        <Reshma Shetty>
-- Create date: <4/2/2013>
-- Description:    <Updates the Flg IsSpecsAvailable in Car Versions table whenever a new version is added in the ItemValues table or deleted from it.>
--Modified By: Reshma Shetty -Also update the IsSpecsExist field
--Modified By: Manish Chourasiya on 11-06-2013 adding condition in update queries and change if condition
--Modified By: Manish Chourasiya on 27-08-2015 added with (nolock)
-- =============================================
CREATE TRIGGER [CD].[TRG_AIUD_IVCarVersion] 
   ON  [CD].[ItemValues]
   AFTER INSERT,DELETE ,UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    DECLARE @InsertedCarVersion INT = NULL;
    DECLARE @DeletedCarVersion INT= NULL ;
    DECLARE @ItemMasterId INT= NULL ;
    
    
    SELECT @InsertedCarVersion = CarVersionId
        ,@ItemMasterId = ItemMasterId
    FROM Inserted
    --WHERE CarVersionId IS NOT NULL

    SELECT @DeletedCarVersion = CarVersionId
    ,@ItemMasterId = ItemMasterId    
    FROM Deleted
    --WHERE CarVersionId IS NOT NULL
        --AND CarVersionId NOT IN (
        --    SELECT DISTINCT CarVersionID
        --    FROM [CD].[ItemValues] WITH(NOLOCK)
        --    )
-- On Insert    
        IF (@DeletedCarVersion IS NULL)
        BEGIN
        -- Set the IsSpecsAvailable and IsSpecsExist
            UPDATE CarVersions
            SET IsSpecsAvailable = 1
                ,IsSpecsExist = 1
            WHERE ID = @InsertedCarVersion
                AND IsSpecsAvailable <> 1
                
        -- Set SpecsSummary

          --  IF (@ItemMasterId IN (12,14,26,29))
          ---If Condition Modified by Manish on 11-06-2013 ---------------------------------------
           IF (@ItemMasterId=12 OR @ItemMasterId=14 OR @ItemMasterId=26  OR @ItemMasterId=29)
                UPDATE CarVersions
                SET SpecsSummary = Specs
                FROM (
                    SELECT DISTINCT CV.Id
                        ,ISNULL(MAX(CASE IV.ItemMasterId WHEN 14 THEN CONVERT(VARCHAR, ItemValue)+ 'cc ' ELSE '' END), '') 
                        + '|'+ISNULL(MAX(CASE IV.ItemMasterId WHEN 26 THEN UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL(MAX(CASE IV.ItemMasterId WHEN 29 THEN ', '+UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL( MAX(CASE IV.ItemMasterId WHEN 12 THEN ', '+CONVERT(VARCHAR, ItemValue)+' kpl' ELSE ''END), '') AS Specs
                    FROM CD.ItemValues IV WITH (NOLOCK)
                    INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = IV.CarVersionId
                    LEFT JOIN CD.UserDefinedMaster UDF WITH (NOLOCK) ON UDF.UserDefinedId = IV.UserDefinedId
                    WHERE IsSpecsAvailable = 1 
                    AND CV.ID=@InsertedCarVersion  ----Condition Add by Manish on 11-06-2013
                    GROUP BY CV.Id
                    ) AS Tab
                WHERE Tab.Id = Carversions.ID
                AND Carversions.ID=@InsertedCarVersion  ----Condition Add by Manish on 11-06-2013
        END
-- On Delete    
        ELSE IF(@InsertedCarVersion IS NULL  )
                BEGIN
                IF(@DeletedCarVersion NOT IN (SELECT DISTINCT CarVersionID FROM [CD].[ItemValues] WITH(NOLOCK)))
                UPDATE CarVersions
                SET IsSpecsAvailable = 0
                    ,IsSpecsExist = 0
                WHERE ID = @DeletedCarVersion
                    AND IsSpecsAvailable <> 0
                            
                            
                IF (@ItemMasterId IN (12,14,26,29))
                UPDATE CarVersions
                SET SpecsSummary = Specs
                FROM (
                    SELECT CV.id,ISNULL(MAX(CASE IV.ItemMasterId WHEN 14 THEN CONVERT(VARCHAR, ItemValue)+ 'cc ' ELSE '' END), '') 
                        + '|'+ISNULL(MAX(CASE IV.ItemMasterId WHEN 26 THEN UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL(MAX(CASE IV.ItemMasterId WHEN 29 THEN ', '+UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL( MAX(CASE IV.ItemMasterId WHEN 12 THEN ', '+CONVERT(VARCHAR, ItemValue)+' kpl' ELSE ''END), '') AS Specs
                    FROM CarVersions CV WITH (NOLOCK) 
                    LEFT JOIN CD.ItemValues IV WITH (NOLOCK) ON CV.ID = IV.CarVersionId
                    LEFT JOIN CD.UserDefinedMaster UDF WITH (NOLOCK) ON UDF.UserDefinedId = IV.UserDefinedId 
                    GROUP BY CV.Id
                    ) AS Tab
                WHERE Tab.Id = Carversions.ID and Carversions.ID=@DeletedCarVersion
                
                END
                    
-- On Update
            ELSE 
            BEGIN
                IF(UPDATE(CarVersionId))
                BEGIN
                UPDATE CarVersions
                SET IsSpecsAvailable = 1
                    ,IsSpecsExist = 1
                WHERE ID = @InsertedCarVersion
                    AND IsSpecsAvailable <> 1
                    
                UPDATE CarVersions
                SET IsSpecsAvailable = 0
                    ,IsSpecsExist = 0
                WHERE ID = @DeletedCarVersion
                    AND IsSpecsAvailable <> 0
                END
                
               -- IF((UPDATE(ItemValue)OR UPDATE(UserDefinedId) OR UPDATE(CustomText)) AND @ItemMasterId IN (12,14,26,29))
               ---If Condition Modified by Manish on 11-06-2013 ---------------------------------------
                IF((UPDATE(ItemValue) OR UPDATE(UserDefinedId) OR UPDATE(CustomText)) AND
                                 (@ItemMasterId=12 OR @ItemMasterId=14 OR @ItemMasterId=26  OR @ItemMasterId=29 ))
                UPDATE CarVersions
                SET SpecsSummary = Specs
                FROM (
                    SELECT DISTINCT CV.Id
                        ,ISNULL(MAX(CASE IV.ItemMasterId WHEN 14 THEN CONVERT(VARCHAR, ItemValue)+ 'cc ' ELSE '' END), '') 
                        + '|'+ISNULL(MAX(CASE IV.ItemMasterId WHEN 26 THEN UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL(MAX(CASE IV.ItemMasterId WHEN 29 THEN ', '+UDF.NAME ELSE ''END), '') 
                         +'|'+ ISNULL( MAX(CASE IV.ItemMasterId WHEN 12 THEN ', '+CONVERT(VARCHAR, ItemValue)+' kpl' ELSE ''END), '') AS Specs
                    FROM CD.ItemValues IV WITH (NOLOCK)
                    INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = IV.CarVersionId
                    LEFT JOIN CD.UserDefinedMaster UDF  WITH (NOLOCK) ON UDF.UserDefinedId = IV.UserDefinedId
                    WHERE IsSpecsAvailable = 1 
                       AND CV.ID=@DeletedCarVersion  ----Condition Add by Manish on 11-06-2013
                    GROUP BY CV.Id
                    ) AS Tab
                WHERE Tab.Id = Carversions.ID
                AND Carversions.ID=@DeletedCarVersion  ----Condition Add by Manish on 11-06-2013
                

             END

END