
  declare @wasteTypeId int

  -- add waste type *** Paper/Board ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Paper/Board')
  set @wasteTypeId = SCOPE_IDENTITY()

  -- add the sub types
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Sorted mixed paper/board', 34.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Unsorted mixed paper/board', 34.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grades 1.04.00, 1.05.00 & 1.05.01', 97.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grade 1.04.01', 70, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grade 1.04.02', 80, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);


  -- add waste type *** Paper ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Paper Composting')
  set @wasteTypeId = SCOPE_IDENTITY()

  -- add the sub types
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Sorted mixed paper/board', 34.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Unsorted mixed paper.board', 34.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grades 1.04.00, 1.05.00 & 1.05.01', 97.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grade 1.04.01', 70, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Grade 1.04.02', 80, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);


  -- add waste type *** Glass ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Glass Remelt')
  set @wasteTypeId = SCOPE_IDENTITY()

  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('100% packaging', 100, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);


  -- add waste type *** Glass ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Glass Other')
  set @wasteTypeId = SCOPE_IDENTITY()

  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('100% packaging', 100, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);
  
  
  -- add waste type *** Aluminium ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Aluminium')
  set @wasteTypeId = SCOPE_IDENTITY()
  
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Extracted from IBA', 87.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Cans and associated packaging', 97.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);
  
  
  -- add waste type *** Steel ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Steel')
  set @wasteTypeId = SCOPE_IDENTITY()
  
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('1+2 mixed', 0.55, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('2', 1.1, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Fragmentised', 4.7, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('4C', 10.6, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('4E', 5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('8B', 10.6, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('steel cans grade 6E', 97.5, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);
  
  
  -- add waste type *** Plastic ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Plastic')
  set @wasteTypeId = SCOPE_IDENTITY()
  
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('HDPE', null, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('PET', null, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Mixed', null, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Packaging film', null, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Pots, tubs, trays', null, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);
  
  
  -- add waste type *** Wood ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Wood')
  set @wasteTypeId = SCOPE_IDENTITY()
  
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('100% packaging', 100, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);
  
  
  -- add waste type *** Wood ***
  insert into [Waste].[dbo].[WasteType] ([Name]) values ('Wood Composting')
  set @wasteTypeId = SCOPE_IDENTITY()
  
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('100% packaging', 100, @wasteTypeId);
  insert into [Waste].[dbo].[WasteSubType] ([Name], [Adjustment], [WasteTypeId]) values ('Other', null, @wasteTypeId);