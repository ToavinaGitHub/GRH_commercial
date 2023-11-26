$(document).ready(function() {
    $('#dataTable').DataTable({
        "pagingType": "full_numbers", // Type de pagination
        "ordering": true, // Tri des colonnes
        "lengthMenu": [[5, 10, 20, -1], [5, 10, 20, "All"]], // Options de nombre d'éléments par page
        "order": [[0, "asc"]], // Colonne de tri par défaut (dans cet exemple, la première colonne)
        
        "language": { // Personnalisation des messages (par exemple, "Affichage de 1 à 10 de 50 éléments")
            "lengthMenu": "Afficher _MENU_ éléments par page",
            "info": "Affichage de _START_ à _END_ de _TOTAL_ éléments",
            "infoEmpty": "Aucun élément à afficher",
            "infoFiltered": "(filtré de _MAX_ éléments au total)",
            "search": "Rechercher :",
            "zeroRecords": "Aucun enregistrement trouvé",
            "paginate": {
                "first": "Premier",
                "last": "Dernier",
                "next": "Suivant",
                "previous": "Précédent"
            }
        }
    });
    $('#dataTable_filter input[type="search"]').css({
      'width': '100%', // Largeur souhaitée
      'height': '30px', // Hauteur souhaitée
      'font-size': '14px', // Taille de police
      'padding': '5px', // Rembourrage
      'border-radius': '5px' // Bordure arrondie
  });
});
