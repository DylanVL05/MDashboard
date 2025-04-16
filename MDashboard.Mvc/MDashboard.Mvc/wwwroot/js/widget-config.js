function inicializarConfiguracionWidgets() {
    // Limpiar cualquier evento anterior para evitar múltiples asignaciones
    $('.config-widget-icon').off('click').on('click', function () {
        const widgetId = $(this).data('widget-id');

        // Mostrar un "loading" en el modal mientras se carga la configuración
        $('#configModalBody').html(`
            <div class="text-center">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Cargando...</span>
                </div>
            </div>
        `);

        // Realizar la petición AJAX para cargar la configuración del widget
        $.get('/Dashboard/ConfigWidget', { id: widgetId }, function (data) {
            $('#configModalBody').html(data);
            const modal = new bootstrap.Modal(document.getElementById('configModal'));
            modal.show();
        }).fail(function () {
            $('#configModalBody').html('<p class="text-danger">Error al cargar la configuración.</p>');
        });
    });
}

$(document).ready(function () {
    // Inicializar la configuración de los widgets al cargar la página
    inicializarConfiguracionWidgets();

    // Manejo del submit del formulario de configuración con AJAX
    $(document).on('submit', 'form[id^="configForm-"]', function (e) {
        e.preventDefault();
        const form = $(this);
        const formData = form.serialize();

        $.post('/Dashboard/UpdateWidget', formData, function () {
            // Recargar solo la sección de widgets sin afectar el modal
            actualizarWidgets();
            // Cerrar el modal después de guardar los cambios
            const modal = bootstrap.Modal.getInstance(document.getElementById('configModal'));
            modal.hide();
        }).fail(function () {
            alert('Hubo un error al guardar los cambios');
        });
    });
});
