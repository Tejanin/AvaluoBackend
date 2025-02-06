INSERT INTO Estados (idtabla, descripcion) VALUES
('Rubrica', 'Activa y sin entregar'),
('Rubrica', 'Activa y entregada'),
('Rubrica', 'Entregada'),
('Rubrica', 'No entregada'),
('Usuario','Activo'),
('Usuario','Desactivado')


INSERT INTO Roles (Descripcion) VALUES
('Profesor'),
('Prueba')

INSERT INTO [dbo].[areas] 
           ([Id], [Descripcion], [IdCoordinador], [FechaCreacion], [UltimaEdicion])  
     VALUES  
           (1, 'Área de Ingeniería', 3, GETDATE(), NULL);  